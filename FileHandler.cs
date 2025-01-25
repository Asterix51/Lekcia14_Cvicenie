using Newtonsoft.Json;

namespace Lekcia14_Cvicenie
{
    public static class FileHandler
    {
        public static void SaveToFile<T>(string fileName, T data)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static T LoadFromFile<T>(string fileName)
        {
            if (!File.Exists(fileName))
                return default;

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
        }
    }
}
