namespace MAUI.Translate.Rcl.Interfaces;

public interface IInputParse
{
    /// <summary>
    /// English comma separated
    /// </summary>
    public const string SupportFileSuffix = ".json";

    Task<IDictionary<string, string>> Parse(Stream stream);

    /// <summary>
    /// Web ContentType, example: application/json
    /// </summary>
    /// <returns></returns>
    string SupportFile();
}