using Newtonsoft.Json;

namespace GaN.Translate.Impl.Translate.Result;

public class BaiduTransResult
{
    /// <summary>
    /// 原文
    /// </summary>
    [JsonProperty("src")]
    public string Src { get; set; }   
    /// <summary>
    /// 译文
    /// </summary>
    [JsonProperty("dst")]
    public string Dst { get; set; }
    
}