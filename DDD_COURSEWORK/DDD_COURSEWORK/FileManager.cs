using System.IO;
using System.Text.Json;
using DDD_COURSEWORK;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class FileManager
{
    const string FilePath = "data.json";

    public static SystemData LoadData()
    {
        if (!File.Exists(FilePath))
        {
            return new SystemData(); 
        }

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<SystemData>(json);
    }

    public static void SaveData(SystemData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(FilePath, json);
    }
}
