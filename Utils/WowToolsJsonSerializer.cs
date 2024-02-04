using Newtonsoft.Json.Linq;

namespace WoWTools.WDBUpdater.Utils
{
    // Original code created by Query
    public class WowToolsJsonSerializer
    {
        public static string Serialize<T>(T value, Newtonsoft.Json.Formatting format = Newtonsoft.Json.Formatting.None)
        {
            // Parse JSON into JObject
            JObject jsonData = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(value, format));

            // Create a dictionary to store key-value pairs for the desired output
            Dictionary<string, JToken> outputData = new Dictionary<string, JToken>();

            // Recursively process JObject properties
            ProcessJObject(jsonData, outputData);

            // Convert the dictionary to a JSON string
            return Newtonsoft.Json.JsonConvert.SerializeObject(outputData, format);
        }

        static void ProcessJObject(JObject jsonObject, Dictionary<string, JToken> outputData, string parentKey = "", string parentKeyIndex = "")
        {
            foreach (var property in jsonObject.Properties())
            {
                string currentKey = string.IsNullOrEmpty(parentKey) ? property.Name + parentKeyIndex : $"{parentKey}{property.Name}{parentKeyIndex}";
                JToken token = property.Value;

                if (token.Type == JTokenType.Object)
                {
                    ProcessJObject((JObject)token, outputData, currentKey, parentKeyIndex);
                }
                else if (token.Type == JTokenType.Array)
                {
                    ProcessJArray((JArray)token, outputData, currentKey, parentKeyIndex);
                }
                else
                {
                    outputData.Add(currentKey, token);
                }
            }
        }

        static void ProcessJArray(JArray jsonArray, Dictionary<string, JToken> outputData, string parentKey, string parentKeyIndex)
        {
            for (int i = 0; i < jsonArray.Count; i++)
            {
                JToken arrayElement = jsonArray[i];
                string currentKey = parentKey;
                string currentKeyIndex = $"[{i}]";

                if (arrayElement.Type == JTokenType.Object)
                {
                    ProcessJObject((JObject)arrayElement, outputData, currentKey, currentKeyIndex);
                }
                else if (arrayElement.Type == JTokenType.Array)
                {
                    ProcessJArray((JArray)arrayElement, outputData, currentKey, currentKeyIndex);
                }
                else
                {
                    outputData.Add($"{currentKey}{currentKeyIndex}", arrayElement);
                }
            }
        }
    }
}
