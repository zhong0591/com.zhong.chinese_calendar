using Newtonsoft.Json;
using System.Diagnostics;

namespace com.zhong.chinese_calendar.Helpers
{
    public class JsonHelper<T>
    {
        public static void SaveToFile(string filePath, List<T> t)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                string jsonString = JsonConvert.SerializeObject(t);
                File.WriteAllText(filePath, jsonString);
            }
            catch (IOException ioex)
            {
                Debug.Write(ioex.Message);
            }
        }

        public static List<T> LoadJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string jsonString = File.ReadAllText(filePath);
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonString) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<T>();
            }
        }
    }
}
