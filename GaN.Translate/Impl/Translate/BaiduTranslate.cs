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
    private readonly string _commonTranslateApi = "https://fanyi-api.baidu.com/api/trans/vip/translate";
    private TranslateKey? auth { get; set; }
    private ILogger<BaiduTranslate> Logger;

    public BaiduTranslate(ILogger<BaiduTranslate> logger)
    {
        Logger = logger;
    }
    
    private string? GetLangDict(LanguageEnum languageEnum)
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

    public async Task<Result<string>> Translate(string input, LanguageEnum sourceLang, LanguageEnum targetLang)
    {
        auth = Settings.GetEngineKey(TranslateEngineEnum.Baidu);
        Debug.Assert(auth != null);
        Debug.Assert(auth.AppId.Length > 0);
        Debug.Assert(auth.Key.Length > 0);

        var param = new BaiduCommonParam(sourceLang, targetLang, input, auth, GetLangDict);
        param.ToSign();

        var appendPathSegments = _commonTranslateApi.SetQueryParam("q", param.Q)
            .SetQueryParam("from", param.From)
            .SetQueryParam("to", param.To)
            .SetQueryParam("appid", param.Auth.AppId)
            .SetQueryParam("salt", param.Salt)
            .SetQueryParam("sign", param.Sign);

        Logger.LogInformation("{} :: {}", nameof(Translate), appendPathSegments.ToString());
        var result = await appendPathSegments.GetJsonAsync<BaiduResult>();

        if (result.ErrorMsg?.Length > 0)
        {
            return new Result<string>(new ApplicationException(result.ErrorMsg));
        }

        var value = result.TransResult?.First().Dst;
        return value != null
            ? new Result<string>(value)
            : new Result<string>(new ApplicationException("result is null"));
    }
}