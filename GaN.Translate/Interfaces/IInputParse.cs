namespace GaN.Translate.Interfaces;

public interface IInputParse
{
    Task<IDictionary<string, string>> Parse(Stream stream);

    /// <summary>
    /// Web ContentType, example: application/json
    /// </summary>
    /// <returns></returns>
    string SupportFile();
}