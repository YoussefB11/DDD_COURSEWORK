using System;
using DDD_COURSEWORK;

class Program
{
    // Program.cs with empty switch logic
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No command provided.");
            return;
        }

        var data = FileManager.LoadData();

        string command = args[0].ToLower();

        switch (command)
        {
            default:
                Console.WriteLine("Unknown command.");
                break;
        }

        FileManager.SaveData(data);
    }



}
