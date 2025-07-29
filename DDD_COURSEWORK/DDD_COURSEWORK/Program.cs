/*
Name: Youssef Baya
Student ID: 202244950
Module: Design develop deploy
Coding language: C# (obviously)
*/

using System;
using System.Linq;
using DDD_COURSEWORK;


public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0) // if no command-line arguments, rnu the interactive menu instead
        {
            RunInteractiveMenu();
            return;
        }

        var data = FileManager.LoadData(); // load existing data from the data.json file
        string command = args[0].ToLower(); // get the first command entered by the user

        switch (command)
        {
            case "checkin": // add a check-in message for a student
                if (args.Length >= 3)
                {
                    string studentId = args[1];
                    string message = string.Join(" ", args[2..]); // combine the message arguments
                    CheckInCommand(data, studentId, message);
                }
                else
                {
                    Console.WriteLine("Usage: checkin <StudentID> <Message>");
                }
                break;

            case "viewstudent": // view a student check-ins and meetings
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

            case "requestmeeting": // Student asks to meet with supervisor
                if (args.Length >= 3)
                {
                    string studentId = args[1];
                    string dateStr = string.Join(" ", args[2..]);
                    RequestMeetingCommand(data, studentId, dateStr);
                }
                else
                {
                    Console.WriteLine("Usage: requestmeeting <StudentID> <DateTime>");
                }
                break;

            case "bookmeeting": // Supervisor books a meeting with student
                if (args.Length >= 4)
                {
                    string studentId = args[1];
                    string dateStr = args[2];
                    string notes = string.Join(" ", args[3..]);
                    BookMeetingCommand(data, studentId, dateStr, notes);
                }
                else
                {
                    Console.WriteLine("Usage: bookmeeting <StudentID> <DateTime> <Notes>");
                }
                break;

            case "viewsummary": // Senior tutor views supervisor engagement
                ViewSummaryCommand(data);
                break;

            default:
                Console.WriteLine("Unknown command."); // print this error if cmd doesn't exist
                break;
        }

        FileManager.SaveData(data); // Save everything at the end to the file
    }

    static void RunInteractiveMenu()
    {
        var data = FileManager.LoadData(); // Load data from the data.sjon file

        Console.WriteLine("-- Student Support System --\n");
        Console.WriteLine("Select your role: ");
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Personal Supervisor");
        Console.WriteLine("3. Senior Tutor");
        Console.Write("Enter choice: ");

        string choice = Console.ReadLine()?.Trim();

        switch (choice)
        {
            case "1":
                RunStudentMenu(data); // Show student options
                break;
            case "2":
                RunSupervisorMenu(data); // Show supervisor options
                break;
            case "3":
                RunTutorMenu(data); // Show tutor options (just one option technically)
                break;
            default:
                Console.WriteLine("Error"); // error message
                break;
        }

        FileManager.SaveData(data); // save 
    }

    static void RunStudentMenu(SystemData data)
    {
        Student student = null;
        string studentId = "";

        // keep asking until the student enters a valid student id (in this case the only valid id is s001 unless we add more data
        // to the json file
        while (student == null)
        {
            Console.Write("Enter your Student ID: ");
            studentId = Console.ReadLine()?.Trim();
            student = data.Students.Find(s => s.Id == studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found so try again.");
            }
        }

        while (true) // prints the student menu
        {
            Console.WriteLine($"Student Menu for {student.Name}");
            Console.WriteLine("1. Submit Check-In");
            Console.WriteLine("2. Request Meeting");
            Console.WriteLine("3. View My Info");
            Console.WriteLine("0. Close");
            Console.Write("Choice: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Console.Write("Enter your check-in message: ");
                    string message = Console.ReadLine();
                    CheckInCommand(data, studentId, message); // save check-in
                    break;
                case "2":
                    Console.Write("Enter the meeting date/time (e.g. 2025-07-25 10:00): ");
                    string dateStr = Console.ReadLine();
                    RequestMeetingCommand(data, studentId, dateStr); // request meeting
                    break;
                case "3":
                    ViewStudentCommand(data, studentId); // show chekc-ins and meetings
                    break;
                case "0":
                    return; // exit
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void RunSupervisorMenu(SystemData data)
    {
        PersonalSupervisor supervisor = null;
        string psId = "";

        // keep asking until a valid supervisor ID is entered
        while (supervisor == null)
        {
            Console.Write("Enter your Supervisor ID: ");
            psId = Console.ReadLine()?.Trim();
            supervisor = data.Supervisors.Find(ps => ps.Id == psId);

            if (supervisor == null)
            {
                Console.WriteLine("PS id not found so try again.");
            }
        }

        while (true) // shows the  supervisor menu
        {
            Console.WriteLine($"Supervisor Menu for {supervisor.Name}");
            Console.WriteLine("1. Book Meeting with Student");
            Console.WriteLine("2. View Student Info");
            Console.WriteLine("0. Close");
            Console.Write("Choose: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Console.Write("Enter Student ID: ");
                    string studentId = Console.ReadLine();
                    Console.Write("Enter meeting date/time: ");
                    string dateStr = Console.ReadLine();
                    Console.Write("Write a note: ");
                    string notes = Console.ReadLine();
                    BookMeetingCommand(data, studentId, dateStr, notes); // book meeting
                    break;
                case "2":
                    Console.Write("Enter Student ID: ");
                    string viewId = Console.ReadLine();
                    ViewStudentCommand(data, viewId); // view a particular student info
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    static void RunTutorMenu(SystemData data)
    {
        while (true) // seniro tutor can view the engagement between students and personal supervisors
        {
            Console.WriteLine("Senior Tutor Menu");
            Console.WriteLine("1. View Supervisors/Students Engagements");
            Console.WriteLine("0. Close");
            Console.Write("Choice: ");
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ViewSummaryCommand(data); // show a report of student engagement with the PS
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    public static void CheckInCommand(SystemData data, string studentId, string message)
    {
        var student = data.Students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        student.CheckIns.Add(new CheckIn
        {
            Date = DateTime.Now, // save time of check-in
            Message = message
        });

        Console.WriteLine($"Check-in submitted for {student.Name}");
    }

    public static void ViewStudentCommand(SystemData data, string studentId)
    {
        var student = data.Students.Find(s => s.Id == studentId); // try to look for the student
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine($"Name: {student.Name}");

        Console.WriteLine("Check-Ins:");
        if (student.CheckIns.Count == 0) // no check-ins saved
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var checkIn in student.CheckIns) // show all check-ins
            {
                Console.WriteLine($"- [{checkIn.Date:yyyy-MM-dd}] {checkIn.Message}");
            }
        }

        Console.WriteLine("Meetings:");
        if (student.Meetings.Count == 0) // no meetings to be shown
        {
            Console.WriteLine("- (none)");
        }
        else
        {
            foreach (var meeting in student.Meetings) // show all meetings
            {
                Console.WriteLine($"- [{meeting.Date:yyyy-MM-dd HH:mm}] With: {meeting.With} | Notes: {meeting.Notes}");
            }
        }
    }

    static void RequestMeetingCommand(SystemData data, string studentId, string dateStr)
    {
        var student = data.Students.Find(s => s.Id == studentId); // find the student
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        if (!DateTime.TryParse(dateStr, out DateTime meetingDate)) // check if the date entred is valid
        {
            Console.WriteLine("Invalid date format. Try something like: 2025-07-23 10:00");
            return;
        }

        var supervisor = data.Supervisors.Find(ps => ps.Id == student.SupervisorId); // find the students persona'l s supervisor 
        if (supervisor == null)
        {
            Console.WriteLine("Supervisor not found.");
            return;
        }

        // add meeting to student’s list and note that the meeting was requested by the student and not the pS
        student.Meetings.Add(new Meeting
        {
            Date = meetingDate,
            With = supervisor.Name,
            Notes = "(Requested by student)"
        });

        Console.WriteLine($"Meeting requested with {supervisor.Name} on {meetingDate:g}");
    }

    public static void BookMeetingCommand(SystemData data, string studentId, string dateStr, string notes)
    {
        var student = data.Students.Find(s => s.Id == studentId); // find student
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        if (!DateTime.TryParse(dateStr, out DateTime meetingDate)) // check if date is valid
        {
            Console.WriteLine("Invalid date format. Use something like: 2025-07-24 14:00");
            return;
        }

        var supervisor = data.Supervisors.Find(ps => ps.Id == student.SupervisorId); // find their PS
        if (supervisor == null)
        {
            Console.WriteLine("Supervisor not found.");
            return;
        }

        // add a new meeting entry with custom notes
        student.Meetings.Add(new Meeting
        {
            Date = meetingDate,
            With = supervisor.Name,
            Notes = notes
        });

        Console.WriteLine($"Meeting booked with {student.Name} on {meetingDate:g} with notes: {notes}");
    }

    static void ViewSummaryCommand(SystemData data)
    {
        foreach (var ps in data.Supervisors) // go through every supervisor
        {
            var students = data.Students.FindAll(s => s.SupervisorId == ps.Id); // get their studenst
            int total = students.Count;
            int withCheckIns = students.Count(s => s.CheckIns.Any()); // students with at least one check-in
            int withMeetings = students.Count(s => s.Meetings.Any()); // students with at least one meeting

            Console.WriteLine($"\nSupervisor: {ps.Name}");
            Console.WriteLine($"- Students assigned: {total}");
            Console.WriteLine($"- Students with check-ins: {withCheckIns}");
            Console.WriteLine($"- Students with meetings: {withMeetings}");
        }
    }
}

