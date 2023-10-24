using GaN.Translate.Interfaces;
using Newtonsoft.Json;

namespace GaN.Translate.Impl.Translate.Result;

public class BaiduTransResult : ITranslateResult
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

    public override string ToString()
    {
        return $"{nameof(Src)}: {Src}, {nameof(Dst)}: {Dst}";
    }

    public string From => Src;
    public string To => Dst;
}