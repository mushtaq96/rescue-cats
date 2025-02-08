public interface IDataService
{
    Task<T> LoadJsonAsync<T>(string fileName);
    Task SaveJsonAsync<T>(string fileName, T data);
}