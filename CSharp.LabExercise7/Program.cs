using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharp.LabExercise7
{
    internal class Program
    {
        class Student: IEquatable<Student>
        {
            public string ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int GradeLevel { get; set; }
            public string Section { get; set; }

            public bool Equals(Student other)
            {
                if (other == null)
                {
                    throw new ArgumentException("Student object cannot be null");
                }

                return this.ID == other.ID;
            }

            public override string ToString()
            {
                return $"{ID},{FirstName},{LastName},{GradeLevel},{Section}";
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }
        }

        class FileManager
        {
            private static void WriteToApplicationLogFile(string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Application Log Timestamp: {DateTime.Now.ToLongTimeString()}");
                }
            }

            public static void CreateApplicationLogFile()
            {
                string logPath = Directory.GetCurrentDirectory() + $"/LogFiles";
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                string filePath = $"{logPath}/{DateTime.Now:MMddyyyy}.log";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                WriteToApplicationLogFile(filePath);
            }

            public static void CreateStudentDBFile(HashSet<Student> students)
            {
                if(students.Count > 0)
                {
                    string dbPath = Directory.GetCurrentDirectory() + $"/StudentDB";
                    if (!Directory.Exists(dbPath))
                    {
                        Directory.CreateDirectory(dbPath);
                    }
                    string filePath = $"{dbPath}/studentdb.{DateTime.Now:MMddyyyyHHmmss}.txt";
                    File.Create(filePath).Dispose();

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        foreach (var student in students)
                        {
                            writer.WriteLine($"{student.ToString()}");
                        }
                    }

                    Console.WriteLine("\nStudents DB File Successfully Created!");
                }
                else
                {
                    Console.WriteLine("\n0 Records Found. No File Created!");
                }
            }

        }

        static void AddStudent(HashSet<Student> students)
        {
            string tryAgain = "y";
            string id;
            string firstName;
            string lastName;
            int gradeLevel;
            string section;

            while (tryAgain.Trim().ToLower().Equals("y"))
            {
                Console.Clear();
                Console.WriteLine("===== ADD NEW STUDENT =====\n");
                Console.Write("Enter Student ID: ");
                id = Console.ReadLine();
                Console.Write("Enter Student Firstname: ");
                firstName = Console.ReadLine();
                Console.Write("Enter Student Lastname: ");
                lastName = Console.ReadLine();
                Console.Write("Enter Student Grade Level: ");
                gradeLevel = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Student Section: ");
                section = Console.ReadLine();

                var newStudent = new Student
                {
                    ID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    GradeLevel = gradeLevel,
                    Section = section
                };
                if (students.Add(newStudent))
                {
                    Console.WriteLine($"\n~~ New Student Added Successfully!");
                    Console.WriteLine($"ID: {newStudent.ID}\nFullname: {newStudent.FirstName} {newStudent.LastName}\nGrade Level: {newStudent.GradeLevel}\nSection: {newStudent.Section}");
                }
                else
                {
                    Console.WriteLine($"\n~~ Student ID ({id}) Already Exists! Please Try Again!");
                }


                Console.Write("\nDo you want to add another Student? (y/n): ");
                tryAgain = Console.ReadLine();
            }

            Console.Write("\nPress any key to return to main menu . . . ");
            Console.ReadKey();
        }

        static void ListStudents(HashSet<Student> students)
        {
            Console.Clear();
            Console.WriteLine("===== STUDENT LIST =====\n");

            if(students.Count > 0)
            {
                foreach (var student in students)
                {
                    //Console.WriteLine($"{student.ToString()}");
                    Console.WriteLine($"ID: {student.ID}, Firstname: {student.FirstName}, Lastname: {student.LastName}, Grade Level: {student.GradeLevel}, Section: {student.Section}");
                }
            }
            else
            {
                Console.WriteLine($"No records found!");
            }

            Console.Write("\nPress any key to return to main menu . . . ");
            Console.ReadKey();
        }

        static Student GetStudentByID(HashSet<Student> students, string id)
        {
            foreach (var student in students)
            {
                if (student.ID == id)
                {
                    return student;
                }
            }

            return null;
        }

        static int GetStudentByName(HashSet<Student> students, string nameSearch)
        {
            int searchCount = 0;
            string searchValues = "";
            foreach (var student in students)
            {
                if (student.FirstName.Contains(nameSearch))
                {
                    searchValues = searchValues + $"[{student.ID}] - Name: {student.FirstName} {student.LastName}, Grade Level: {student.GradeLevel}, Section: {student.Section}\n";
                    searchCount++;
                }
            }

            if (searchCount > 0)
            {
                Console.WriteLine($"\n{searchCount} Record/s Found!");
                Console.WriteLine(searchValues);
            }
            else
            {
                Console.WriteLine("\n0 Records Found! Try Again!");
            }

            return searchCount;
        }

        static void ModifyStudent(HashSet<Student> students)
        {
            string tryAgain = "y";

            while (tryAgain.Trim().ToLower().Equals("y"))
            {
                Student searchedStudent;
                string inputID;
                Console.Clear();
                Console.WriteLine("===== MODIFY STUDENT RECORD =====\n");
                Console.WriteLine("[1] - Search by ID");
                Console.WriteLine("[2] - Search by Name");
                Console.Write("\nEnter Selection: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("===== SEARCH RECORD BY ID (MODIFY) =====\n");
                        Console.Write("Enter ID: ");
                        inputID = Console.ReadLine();

                        searchedStudent = GetStudentByID(students, inputID);

                        if (searchedStudent != null)
                        {
                            Console.WriteLine($"\nSearch Result:\nID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}\n");

                            Console.Write("New Student Firstname: ");
                            string firstName = Console.ReadLine();
                            Console.Write("New Student Lastname: ");
                            string lastName = Console.ReadLine();
                            Console.Write("New Student Grade Level: ");
                            int gradeLevel = Convert.ToInt32(Console.ReadLine());
                            Console.Write("New Student Section: ");
                            string section = Console.ReadLine();

                            var modifiedStudent = new Student
                            {
                                ID = inputID,
                                FirstName = firstName,
                                LastName = lastName,
                                GradeLevel = gradeLevel,
                                Section = section
                            };
                            
                            students.Remove(searchedStudent);
                            students.Add(modifiedStudent);

                            Console.WriteLine("\nStudent Successfully Updated!");
                            Console.WriteLine($"FROM: ID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}");
                            Console.WriteLine($"TO: ID: {modifiedStudent.ID}, Firstname: {modifiedStudent.FirstName}, Lastname: {modifiedStudent.LastName}, Grade Level: {modifiedStudent.GradeLevel}, Section: {modifiedStudent.Section}");
                        }
                        else
                        {
                            Console.WriteLine($"\nStudent with id ({inputID}) doesn't exist! Modification Cancelled!");
                        }

                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("===== SEARCH RECORD BY NAME (MODIFY) =====\n");
                        Console.Write("Enter Student Name: ");
                        string nameSearch = Console.ReadLine();

                        if(GetStudentByName(students, nameSearch) > 0)
                        {
                            Console.Write("Select Student ID: ");
                            inputID = Console.ReadLine();

                            searchedStudent = GetStudentByID(students, inputID);

                            if (searchedStudent != null)
                            {
                                Console.WriteLine($"\nSearch Result:\nID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}\n");

                                Console.Write("New Student Firstname: ");
                                string firstName = Console.ReadLine();
                                Console.Write("New Student Lastname: ");
                                string lastName = Console.ReadLine();
                                Console.Write("New Student Grade Level: ");
                                int gradeLevel = Convert.ToInt32(Console.ReadLine());
                                Console.Write("New Student Section: ");
                                string section = Console.ReadLine();

                                var modifiedStudent = new Student
                                {
                                    ID = inputID,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    GradeLevel = gradeLevel,
                                    Section = section
                                };

                                students.Remove(searchedStudent);
                                students.Add(modifiedStudent);

                                Console.WriteLine("\nStudent Successfully Updated!");
                                Console.WriteLine($"FROM: ID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}");
                                Console.WriteLine($"TO: ID: {modifiedStudent.ID}, Firstname: {modifiedStudent.FirstName}, Lastname: {modifiedStudent.LastName}, Grade Level: {modifiedStudent.GradeLevel}, Section: {modifiedStudent.Section}");
                            }
                            else
                            {
                                Console.WriteLine($"\nStudent with id ({inputID}) doesn't exist! Modification Cancelled!");
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("\nInvalid Selection! Modify Cancelled!");
                        break;

                }

                Console.Write("\nDo you want to Modify another Student? (y/n): ");
                tryAgain = Console.ReadLine();
            }

            Console.Write("\nPress any key to return to main menu . . . ");
            Console.ReadKey();
        }

        static void DeleteStudent(HashSet<Student> students)
        {
            string tryAgain = "y";

            while (tryAgain.Trim().ToLower().Equals("y"))
            {
                string inputID;
                Student searchedStudent;
                Console.Clear();
                Console.WriteLine("===== DELETE STUDENT RECORD =====\n");
                Console.WriteLine("[1] - Search by ID");
                Console.WriteLine("[2] - Search by Name");
                Console.Write("\nEnter Selection: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("===== SEARCH RECORD BY ID (DELETE) =====\n");
                        Console.Write("Enter ID: ");
                        inputID = Console.ReadLine();

                        searchedStudent = GetStudentByID(students, inputID);

                        if (searchedStudent != null)
                        {
                            Console.WriteLine($"\nSearch Result:\nID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}\n");

                            Console.Write("Enter [y] to confirm deletion: ");
                            if(Console.ReadLine().ToLower() == "y")
                            {
                                students.Remove(searchedStudent);
                                Console.WriteLine($"\nStudent with id ({searchedStudent.ID}) is successfully deleted!");
                            }
                            else
                            {
                                Console.WriteLine("Deletion Cancelled!");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"\nStudent with id ({inputID}) doesn't exist! Deletion Cancelled");
                        }

                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("===== SEARCH RECORD BY NAME (DELETE) =====\n");
                        Console.Write("Enter Student Name: ");
                        string nameSearch = Console.ReadLine();

                        if (GetStudentByName(students, nameSearch) > 0)
                        {
                            Console.Write("Select Student ID: ");
                            inputID = Console.ReadLine();

                            searchedStudent = GetStudentByID(students, inputID);

                            if (searchedStudent != null)
                            {
                                Console.WriteLine($"\nSearch Result:\nID: {searchedStudent.ID}, Firstname: {searchedStudent.FirstName}, Lastname: {searchedStudent.LastName}, Grade Level: {searchedStudent.GradeLevel}, Section: {searchedStudent.Section}\n");

                                Console.Write("Enter [y] to confirm deletion: ");
                                if (Console.ReadLine().ToLower() == "y")
                                {
                                    students.Remove(searchedStudent);
                                    Console.WriteLine($"\nStudent with id ({searchedStudent.ID}) is successfully deleted!");
                                }
                                else
                                {
                                    Console.WriteLine("Deletion Cancelled!");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"\nStudent with id ({inputID}) doesn't exist! Deletion Cancelled");
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Selection! Deletion Cancelled!");
                        break;

                }

                Console.Write("\nDo you want to Delete another Student? (y/n): ");
                tryAgain = Console.ReadLine();
            }

            Console.Write("\nPress any key to return to main menu . . . ");
            Console.ReadKey();
        }

            static void Main(string[] args)
        {
            FileManager.CreateApplicationLogFile();
            HashSet<Student> students = new HashSet<Student>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== STUDENT DATABASE MANAGEMENT SYSTEM =====\n");
                Console.WriteLine("[1] - Add\tRecords");
                Console.WriteLine("[2] - List\tRecords");
                Console.WriteLine("[3] - Modify\tRecords");
                Console.WriteLine("[4] - Delete\tRecords");
                Console.WriteLine("[5] - Exit\tProgram");
                Console.Write("\nSelect Your Choice:=> ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        AddStudent(students);
                        break;
                    case "2":
                        Console.Clear();
                        ListStudents(students);
                        break;
                    case "3":
                        Console.Clear();
                        ModifyStudent(students);
                        break;
                    case "4":
                        Console.Clear();
                        DeleteStudent(students);
                        break;
                    case "5":
                        FileManager.CreateStudentDBFile(students);
                        Console.WriteLine("\nApplication Terminated!");
                        Environment.Exit(-1);
                        break;
                    default:
                        Console.Write("\nInvalid Selection! Press Any Key To Try Again! . . . ");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
