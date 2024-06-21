using Newtonsoft.Json;

namespace MAUI.Translate.Rcl.Impl.Translate.Result;

public class BaiduResult
{
    /// <summary>
    /// 源语言
    /// </summary>
    [JsonProperty("from")]
    public string? From { get; set; }

    /// <summary>
    /// 目标语言
    /// </summary>
    [JsonProperty("to")]
    public string? To { get; set; }

    /// <summary>
    /// 翻译结果
    /// </summary>
    [JsonProperty("trans_result")]
    public IEnumerable<BaiduTransResult>? TransResult { get; set; }

    [JsonProperty("error_result")] public string? ErrorCode { get; set; }
    [JsonProperty("error_msg")] public string? ErrorMsg { get; set; }
}