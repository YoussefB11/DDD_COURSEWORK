using System.IO;
using System.Text.Json;
using DDD_COURSEWORK;
using static System.Runtime.InteropServices.JavaScript.JSType;

// This class handles saving and loading the system data from the JSON file
public static class FileManager
{
    static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "data.json"); // path to the data file

    public static SystemData LoadData()
    {
        // If the file doesn't exist yet, we just return an empty system
        if (!File.Exists(FilePath))
        {
            return new SystemData();
        }

        // read the contents of the file and turn it into a systemdtaa object
        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<SystemData>(json);
    }

    public static void SaveData(SystemData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        // this turn the system data into a JSON string and write it to our json file (data.json)
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(FilePath, json);
    }
}
