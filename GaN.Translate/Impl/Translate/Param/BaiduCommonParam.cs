using System.Security.Cryptography;
using System.Text;
using GaN.Translate.Dto;
using GaN.Translate.Enums;
using Newtonsoft.Json;

namespace GaN.Translate.Impl.Translate.Param;

public class BaiduCommonParam : BaseParam
{
    [JsonIgnore]
    private readonly Func<LanguageEnum, string?> _langConvert;

    public BaiduCommonParam(LanguageEnum fromLang, LanguageEnum toLang, string input, TranslateKey auth,Func<LanguageEnum,string?> langConvert)
    {
        _langConvert = langConvert;
        FromLang = fromLang;
        ToLang = toLang;
        Q = input;
        Input = input;
        Auth = auth;

        Salt = Random.Shared.Next(100000000,999999999).ToString();
     
    }

    /// <summary>
    /// 请求翻译query
    /// </summary>
    public string Q { get; set; }
    [JsonIgnore]
    public TranslateKey Auth { get; }

    /// <summary>
    /// 随机数
    /// </summary>
    public string Salt { get; set; }
    /// <summary>
    /// 签名, 调用ToSign来赋值
    /// </summary>
    public string? Sign { get; set; }
    
    public string ToSign()
    {
        var bytes = Encoding.UTF8.GetBytes($"{Auth.AppId}{Q}{Salt}{Auth.Key}");
        var sb = new StringBuilder();
        foreach (var b in MD5.HashData(bytes))
        {
            sb.Append(b.ToString("x2"));
            
        }
        var result = sb.ToString();
        Sign = result;
        return result;
    }
    [JsonIgnore]
    public LanguageEnum FromLang { get; set; }
    
    public string? From => _langConvert.Invoke(FromLang);
    public string? To => _langConvert.Invoke(ToLang);
    
    [JsonIgnore]
    public LanguageEnum ToLang { get; set; }
    [JsonIgnore]
    public string Input { get; set; }
    
    
}