using System.Text.Json;

namespace CalculatorGrade
{
    static class Program
    {
        static string filePath = "Students.json";
        static void Main(string[] args)
        {

            List<Student> students = LoadFromFile();
            HandleChoice(students);
        }

        static void ShowMenu()
        {
            Console.WriteLine("1. Input new Student");
            Console.WriteLine("2. Show all Students");
            Console.WriteLine("3. Exit");
            Console.WriteLine("4. Search Student");
            Console.WriteLine("5. Show average score");
            Console.WriteLine("6. Save");
            Console.WriteLine("7. Load");
            Console.Write("Select an option: ");
        }

        static void HandleChoice(List<Student> students)
        {
            while (true)
            {
                ShowMenu();
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.WriteLine("Input new Student");
                    string name = GetName();
                    double score = GetScore();
                    AddStudent(students, name, score);
                    SaveFile(students);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Show all Students");
                    foreach (var student in students)
                    {
                        PrintResult(student.Name, student.Score);
                    }
                }
                else if (choice == "3")
                {
                    return;
                }
                else if (choice == "4")
                {
                    Console.Write("Search Student Name : ");
                    string name = Console.ReadLine() ?? "";
                    SearchStudent(students, name);
                }
                else if (choice == "5")
                {
                    CalculateAverageScore(students);
                }
                else if (choice == "6")
                {
                    SaveFile(students);
                }
                else if (choice == "7")
                {
                    var loaded = LoadFromFile();

                    students.Clear();
                    students.AddRange(loaded);

                    Console.WriteLine("Loaded successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                }
            }
        }

        static string GetName()
        {
            while (true)
            {
                Console.Write("Input name : ");
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                    continue;
                }
                if (int.TryParse(name, out int result))
                {
                    Console.WriteLine($"{result} is not a name. Please enter a valid name.");
                    continue;
                }

                return name;
            }
        }

        static double GetScore()
        {

            while(true)
            {
                Console.Write("Input score : ");

                if (!Double.TryParse(Console.ReadLine(), out double score))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                    continue;
                }

                if (!IsValidScore(score))
                {
                    Console.WriteLine("Score must be between 0 and 100.");
                    continue;
                }

                return score;
            }
        }

        static bool IsValidScore(double score)
        {
            return score >= 0 && score <= 100;
        }

        static char CalculateGrade(double score)
        {
            return score switch
            {
                >= 80 => 'A',
                >= 70 => 'B',
                >= 60 => 'C',
                >= 50 => 'D',
                _ => 'F',
            };
        }

        static void PrintResult(string name, double score)
        {
            char grade = CalculateGrade(score);
            Console.WriteLine($"Name: {name}\nScore: {score}\nGrade: {grade}\n");
        }

        static void AddStudent(List<Student> students, string name, double score)
        {
            students.Add(new Student() { Name = name, Score = score });
        }

        static void SearchStudent(List<Student> student, string name)
        {
            var foundStudent = student.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (foundStudent.Any())
            {
                foreach (var s in foundStudent)
                {
                    PrintResult(s.Name, s.Score);
                }
            } else
            {
                Console.WriteLine("Student not found.");
            }
        }

        static void CalculateAverageScore(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No students available to calculate average score.\n");
            } else
            {
                double totalScore = students.Sum(s => s.Score);
                double averageScore = totalScore / students.Count;
                Console.WriteLine($"Average Score: {averageScore}\n");
            }
        }

        static void SaveFile(List<Student> students)
        {
            try
            {
                var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(filePath, json);

                Console.WriteLine("Saved Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save filled: {ex.Message}");
            }
        }

        static List<Student> LoadFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Student>();

                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load file. {ex.Message}");
                return new List<Student>();
            }
        }
    }
}


