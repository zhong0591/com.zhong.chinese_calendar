using com.zhong.chinese_calendar.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace com.zhong.chinese_calendar.Services
{
    public class JsonFileService<T> : IJsonFileService<T>
    { 
        public void SaveToFile(string filePath, List<T> t)
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

        public List<T> LoadJson(string filePath)
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
