using Flurl.Util;
using GaN.Translate.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GaN.Translate.Impl.Parse;

public class JsonParse : IInputParse
{
    public async Task<IDictionary<string, string>> Parse(Stream stream)
    {
        IDictionary<string, string> inputFileDataMap = new Dictionary<string, string>(128);

        var cb = new ConfigurationBuilder();
        var configurationRoot = cb.AddJsonStream(stream).Build();

        return inputFileDataMap;
    }

    public string SupportFile()
    {
        return "application/json";
    }
    
public static JObject ToJson(IEnumerable<IConfigurationSection> configurationSections)
{
    JContainer result = null;
    var setting = new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Merge};
    var enumerable = configurationSections as IConfigurationSection[] ?? configurationSections.ToArray();
    // 檢查是否是 array：value 不為 null 表示為最底層; array 的元素 key 會是 int (可能出現誤判);最後是 path 扣掉 int 的部份如果都是相同值就進一步確認為 array
    if (enumerable.All(a => a.Value != null
                            && int.TryParse(a.Key, out var _))
        && enumerable.Select(a => a.Path.Split(":").Reverse().Skip(1).First()).Distinct().Count() == 1)
    {
        var tmpList = new List<string>();
        tmpList.AddRange(enumerable.Select(a => a.Value));
        //取得 array 名稱
        var key = enumerable.Select(a => a.Path.Split(":").Reverse().Skip(1).First()).First();
        result = new JObject {{key, JToken.FromObject(tmpList)}};
        return (JObject) result;
    }
    foreach (var child in enumerable)
    {
        var obj = new JObject();
        if (string.IsNullOrWhiteSpace(child.Value))
        {
            var toJson = ToJson(child.GetChildren());
            // 如果是 array 的話，就換掉整個 object，避免多一層相同 property name
            if (toJson.ContainsKey(child.Key))
            {
                obj = toJson;
            }
            else
            {
                obj.Add(child.Key, ToJson(child.GetChildren()));
            }
        }
        else
        {
            //處理轉型，這邊只寫了 int 與 bool
            if (int.TryParse(child.Value, out var intValue))
            {
                obj.Add(child.Key, intValue);
            }
            else if (bool.TryParse(child.Value, out var boolValue))
            {
                obj.Add(child.Key, boolValue);
            }
            else
            {
                obj.Add(child.Key, child.Value);
            }
        }
        if (result == null)
        {
            result = obj;
        }
        else
        {
            result.Merge(obj, setting);
        }
    }
    return (JObject) result;
}
}

