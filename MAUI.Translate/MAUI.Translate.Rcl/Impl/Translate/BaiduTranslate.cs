using System.Diagnostics;
using Flurl;
using Flurl.Http;
using MAUI.Translate.Rcl.Dto;
using MAUI.Translate.Rcl.Enums;
using MAUI.Translate.Rcl.Impl.Translate.Param;
using MAUI.Translate.Rcl.Impl.Translate.Result;
using MAUI.Translate.Rcl.Interfaces;
using MAUI.Translate.Rcl.Pages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MAUI.Translate.Rcl.Impl.Translate;

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

    public async Task<IEnumerable<ITranslateResult>> Translate(string input, LanguageEnum sourceLang,
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
        var result0 = await appendPathSegments.GetStringAsync();
        var result = JsonConvert.DeserializeObject<BaiduResult>(result0);

        if (result.ErrorMsg?.Length > 0)
        {
            throw new ApplicationException(result.ErrorMsg);
        }

        _logger.LogInformation("translate result :: {}", result.TransResult);
        var value = result.TransResult;
        return value ?? throw new ApplicationException("result is null");
    }
}