using MAUI.Translate.Rcl.Enums;

namespace MAUI.Translate.Rcl.Impl.Translate.Param;

public interface BaseParam
{
    public LanguageEnum FromLang { get; set; }
    public LanguageEnum ToLang { get; set; }
    public string Input { get; set; }
}