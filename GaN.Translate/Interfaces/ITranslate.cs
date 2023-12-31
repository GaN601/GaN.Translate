﻿using System.Diagnostics.SymbolStore;
using BlazorComponent;
using DotNext;
using GaN.Translate.Enums;

namespace GaN.Translate.Interfaces;

public interface ITranslate
{
    public TranslateEngineEnum EngineEnum { get; }
    public Task<Result<IEnumerable<ITranslateResult>>> Translate(string input,LanguageEnum sourceLang, LanguageEnum targetLang);
    
}