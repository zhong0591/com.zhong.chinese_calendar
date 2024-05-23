namespace com.zhong.chinese_calendar.Interfaces
{
    public interface IJsonFileService<T>  
    {
        void SaveToFile(string filePath, List<T> t); 
        List<T> LoadJson(string filePath);
    }
}
