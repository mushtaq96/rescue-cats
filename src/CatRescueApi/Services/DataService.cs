using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class DataService : IDataService
{
    protected readonly string _dataPath;

    public DataService(string dataPath = "./Data")
    {
        _dataPath = dataPath;
        Directory.CreateDirectory(_dataPath);
    }

    public async Task<T> LoadJsonAsync<T>(string fileName)
    {
        var filePath = Path.Combine(_dataPath, $"{fileName}.json");
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "{}");

        var json = await File.ReadAllTextAsync(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task SaveJsonAsync<T>(string fileName, T data)
    {
        var filePath = Path.Combine(_dataPath, $"{fileName}.json");
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, json);
    }
}