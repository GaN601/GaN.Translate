using System.Diagnostics;
using DotNext;
using Flurl;
using Flurl.Http;
using GaN.Translate.Dto;
using GaN.Translate.Enums;
using GaN.Translate.Impl.Translate.Param;
using GaN.Translate.Impl.Translate.Result;
using GaN.Translate.Interfaces;
using GaN.Translate.Pages;
using Microsoft.Extensions.Logging;

namespace GaN.Translate.Impl.Translate;

public class BaiduTranslate
    : ITranslate
{
    private const string CommonTranslateApi = "https://fanyi-api.baidu.com/api/trans/vip/translate";
    private TranslateKey? Auth { get; set; }
    private readonly ILogger<BaiduTranslate> _logger;

    public BaiduTranslate(ILogger<BaiduTranslate> logger)
    {
        _logger = logger;
    }

    private static string GetLangDict(LanguageEnum languageEnum)
    {
        return languageEnum switch
        {
            LanguageEnum.Auto => "auto",
            LanguageEnum.ZhCn => "zh",
            LanguageEnum.ZhTw => "cht",
            LanguageEnum.ZhHk => "yue",
            LanguageEnum.EnUs => "en",
            LanguageEnum.JaJp => "jp",
            LanguageEnum.KoKr => "kor",
            _ => throw new NotImplementedException("LanguageEnum data map no impl")
        };
    }

    public TranslateEngineEnum EngineEnum => TranslateEngineEnum.Baidu;

    public async Task<Result<IEnumerable<ITranslateResult>>> Translate(string input, LanguageEnum sourceLang,
        LanguageEnum targetLang)
    {
        Auth = Settings.GetEngineKey(TranslateEngineEnum.Baidu);
        Debug.Assert(Auth != null);
        Debug.Assert(Auth.AppId.Length > 0);
        Debug.Assert(Auth.Key.Length > 0);

        var param = new BaiduCommonParam(sourceLang, targetLang, input, Auth, GetLangDict);
        param.ToSign();

        var appendPathSegments = CommonTranslateApi.SetQueryParam("q", param.Q)
            .SetQueryParam("from", param.From)
            .SetQueryParam("to", param.To)
            .SetQueryParam("appid", param.Auth.AppId)
            .SetQueryParam("salt", param.Salt)
            .SetQueryParam("sign", param.Sign);

        _logger.LogInformation("{} :: {}", nameof(Translate), appendPathSegments);
        var result = await appendPathSegments.GetJsonAsync<BaiduResult>();

        if (result.ErrorMsg?.Length > 0)
        {
            return new Result<IEnumerable<ITranslateResult>>(new ApplicationException(result.ErrorMsg));
        }

        _logger.LogInformation("translate result :: {}", result.TransResult);
        var value = result.TransResult;
        return value != null
            ? new Result<IEnumerable<ITranslateResult>>(value)
            : new Result<IEnumerable<ITranslateResult>>(new ApplicationException("result is null"));
    }
}