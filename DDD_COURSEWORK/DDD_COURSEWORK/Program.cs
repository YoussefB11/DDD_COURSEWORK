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

            case "viewstudent":
                if (args.Length >= 2)
                {
                    string studentId = args[1];
                    ViewStudentCommand(data, studentId);
                }
                else
                {
                    Console.WriteLine("Usage: viewstudent <StudentID>");
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

    static void ViewStudentCommand(SystemData data, string studentId)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine($"Name: {student.Name}");
        Console.WriteLine("Check-Ins:");
        if (student.CheckIns.Count == 0)
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var checkIn in student.CheckIns)
            {
                Console.WriteLine($"- [{checkIn.Date:yyyy-MM-dd}] {checkIn.Message}");
            }
        }

        Console.WriteLine("Meetings:");
        if (student.Meetings.Count == 0)
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var meeting in student.Meetings)
            {
                Console.WriteLine($"- [{meeting.Date:yyyy-MM-dd HH:mm}] With: {meeting.With} | Notes: {meeting.Notes}");
            }
        }
    }


}
