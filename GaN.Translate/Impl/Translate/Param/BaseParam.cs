using GaN.Translate.Enums;

namespace GaN.Translate.Impl.Translate.Param;

public interface BaseParam
{
   

    public LanguageEnum FromLang { get; set; }
    public LanguageEnum ToLang { get; set; }
    public string Input { get; set; }
}