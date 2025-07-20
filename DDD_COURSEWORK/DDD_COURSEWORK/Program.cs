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
            case "checkin":
                if (args.Length >= 3)
                {
                    string studentId = args[1];
                    string message = string.Join(" ", args[2..]);
                    CheckInCommand(data, studentId, message);
                }
                else
                {
                    Console.WriteLine("Usage: checkin <StudentID> <Message>");
                }
                break;


            default:
                Console.WriteLine("Unknown command.");
                break;
        }

        FileManager.SaveData(data);
    }

    static void CheckInCommand(SystemData data, string studentId, string message)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        student.CheckIns.Add(new CheckIn
        {
            Date = DateTime.Now,
            Message = message
        });

        Console.WriteLine($"Check-in submitted for {student.Name}");
    }



}
