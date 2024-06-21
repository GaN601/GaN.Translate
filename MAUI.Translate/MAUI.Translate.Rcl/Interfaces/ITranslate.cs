using MAUI.Translate.Rcl.Enums;

namespace MAUI.Translate.Rcl.Interfaces;

public interface ITranslate
{
    public TranslateEngineEnum EngineEnum { get; }

    public Task<IEnumerable<ITranslateResult>>
        Translate(string input, LanguageEnum sourceLang, LanguageEnum targetLang);
}