using GaN.Translate.Interfaces;
using Newtonsoft.Json;

namespace GaN.Translate.Impl.Parse;

public class JsonParse : IInputParse
{
    public async Task<IDictionary<string, string>> Parse(Stream stream)
    {
        IDictionary<string, string> inputFileDataMap = new Dictionary<string, string>(128);

        using var streamReader = new StreamReader(stream);
        using var jsonRender = new JsonTextReader(streamReader);

        while (await jsonRender.ReadAsync())
        {
            if (jsonRender.TokenType != JsonToken.PropertyName) continue;
            var key = jsonRender.Value.ToString();
            if (key is not { Length: > 0 }) continue;

            await jsonRender.ReadAsync();
            var value = jsonRender.Value.ToString();
            if (value is not { Length: > 0 }) continue;

            inputFileDataMap[key] = value;
        }

        return inputFileDataMap;
    }

    public string SupportFile()
    {
        return "application/json";
    }
}