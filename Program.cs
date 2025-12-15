using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement;

class Program
{
    static void Main()
    {
        Logger.Info("Application started");
        var storage = new StorageService();
        var service = new StudentService(storage);

        while (true)
        {
            try
            {
                Console.WriteLine("\n=== Student Management System - Enhanced ===");
                Console.WriteLine("1.  Add Student");
                Console.WriteLine("2.  View All Students");
                Console.WriteLine("3.  Search Student by ID");
                Console.WriteLine("4.  View Topper");
                Console.WriteLine("5.  Update Student");
                Console.WriteLine("6.  Delete Student");
                Console.WriteLine("7.  Sort Students");
                Console.WriteLine("8.  Filter Students");
                Console.WriteLine("9.  View Statistics");
                Console.WriteLine("10. Enhanced Search");
                Console.WriteLine("11. Exit");
                Console.Write("\nChoose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Logger.Warning("Invalid input provided");
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddStudent(service);
                        break;
                    case 2:
                        ViewAll(service);
                        break;
                    case 3:
                        SearchStudent(service);
                        break;
                    case 4:
                        ShowTopper(service);
                        break;
                    case 5:
                        UpdateStudent(service);
                        break;
                    case 6:
                        DeleteStudent(service);
                        break;
                    case 7:
                        SortStudents(service);
                        break;
                    case 8:
                        FilterStudents(service);
                        break;
                    case 9:
                        ViewStatistics(service);
                        break;
                    case 10:
                        EnhancedSearch(service);
                        break;
                    case 11:
                        Logger.Info("Application closed by user");
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1-11.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Unexpected error in main loop: {ex.Message}");
                Console.WriteLine($"An unexpected error occurred. Please try again.");
            }
        }
    }

    static void AddStudent(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Add New Student ---");
            Console.Write("Enter ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: ID must be a valid number.");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Error: Age must be a valid number.");
                return;
            }

            Console.Write("Enter Department: ");
            string dept = Console.ReadLine();

            Console.Write("Enter marks (comma-separated): ");
            string[] marksInput = Console.ReadLine().Split(',');
            var marks = new List<int>();

            foreach (var m in marksInput)
            {
                if (!int.TryParse(m.Trim(), out int mark))
                {
                    Console.WriteLine($"Error: '{m.Trim()}' is not a valid mark.");
                    return;
                }
                marks.Add(mark);
            }

            var student = new Student(id, name, age, dept, marks);
            if (!service.AddStudent(student))
                Console.WriteLine("Error: Student ID already exists.");
            else
                Console.WriteLine("✓ Student added successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in AddStudent: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ViewAll(StudentService service)
    {
        try
        {
            var students = service.GetAllStudents();
            if (students.Count == 0)
            {
                Console.WriteLine("\nNo students found.");
                return;
            }

            Console.WriteLine($"\n--- All Students (Total: {students.Count}) ---");
            foreach (var student in students)
                Console.WriteLine(student);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in ViewAll: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SearchStudent(StudentService service)
    {
        try
        {
            Console.Write("\nEnter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var student = service.GetStudentById(id);
            if (student == null)
                Console.WriteLine("Student not found.");
            else
                Console.WriteLine($"\n{student}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in SearchStudent: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ShowTopper(StudentService service)
    {
        try
        {
            var topper = service.GetTopper();
            if (topper == null)
                Console.WriteLine("\nNo students available.");
            else
                Console.WriteLine($"\n🏆 Topper: {topper}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in ShowTopper: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void UpdateStudent(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Update Student ---");
            Console.Write("Enter Student ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var existing = service.GetStudentById(id);
            if (existing == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Current Details: {existing}");
            Console.WriteLine("\nEnter new details (press Enter to keep current value):");

            Console.Write($"New ID [{existing.Id}]: ");
            string newIdInput = Console.ReadLine();
            int newId = string.IsNullOrWhiteSpace(newIdInput) ? existing.Id : int.Parse(newIdInput);

            Console.Write($"New Name [{existing.Name}]: ");
            string newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
                newName = existing.Name;

            Console.Write($"New Age [{existing.Age}]: ");
            string newAgeInput = Console.ReadLine();
            int newAge = string.IsNullOrWhiteSpace(newAgeInput) ? existing.Age : int.Parse(newAgeInput);

            Console.Write($"New Department [{existing.Department}]: ");
            string newDept = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDept))
                newDept = existing.Department;

            Console.Write($"New Marks (comma-separated) [{string.Join(",", existing.Marks)}]: ");
            string newMarksInput = Console.ReadLine();
            List<int> newMarks;
            if (string.IsNullOrWhiteSpace(newMarksInput))
            {
                newMarks = existing.Marks;
            }
            else
            {
                newMarks = newMarksInput.Split(',').Select(m => int.Parse(m.Trim())).ToList();
            }

            var updatedStudent = new Student(newId, newName, newAge, newDept, newMarks);
            if (service.UpdateStudent(id, updatedStudent))
                Console.WriteLine("✓ Student updated successfully.");
            else
                Console.WriteLine("Failed to update student.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in UpdateStudent: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void DeleteStudent(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Delete Student ---");
            Console.Write("Enter Student ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var student = service.GetStudentById(id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Student to delete: {student}");
            Console.Write("Are you sure? (yes/no): ");
            string confirmation = Console.ReadLine();

            if (confirmation?.Trim().ToLower() == "yes")
            {
                if (service.DeleteStudent(id))
                    Console.WriteLine("✓ Student deleted successfully.");
                else
                    Console.WriteLine("Failed to delete student.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in DeleteStudent: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SortStudents(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Sort Students ---");
            Console.WriteLine("1. Sort by Name");
            Console.WriteLine("2. Sort by Age");
            Console.WriteLine("3. Sort by Average Marks");
            Console.Write("Choose sorting option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            List<Student> sorted;
            switch (choice)
            {
                case 1:
                    sorted = service.GetStudentsSortedByName();
                    Console.WriteLine("\n--- Students Sorted by Name ---");
                    break;
                case 2:
                    sorted = service.GetStudentsSortedByAge();
                    Console.WriteLine("\n--- Students Sorted by Age ---");
                    break;
                case 3:
                    sorted = service.GetStudentsSortedByAverage();
                    Console.WriteLine("\n--- Students Sorted by Average Marks ---");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            if (sorted.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            foreach (var student in sorted)
                Console.WriteLine(student);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in SortStudents: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void FilterStudents(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Filter Students ---");
            Console.WriteLine("1. Filter by Department");
            Console.WriteLine("2. Filter by Minimum Average Marks");
            Console.Write("Choose filtering option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            List<Student> filtered;
            switch (choice)
            {
                case 1:
                    Console.Write("Enter Department: ");
                    string dept = Console.ReadLine();
                    filtered = service.FilterByDepartment(dept);
                    Console.WriteLine($"\n--- Students in {dept} Department ---");
                    break;
                case 2:
                    Console.Write("Enter Minimum Average: ");
                    if (!double.TryParse(Console.ReadLine(), out double minAvg))
                    {
                        Console.WriteLine("Invalid average.");
                        return;
                    }
                    filtered = service.FilterByMinimumAverage(minAvg);
                    Console.WriteLine($"\n--- Students with Average >= {minAvg:F2} ---");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            if (filtered.Count == 0)
            {
                Console.WriteLine("No students match the filter criteria.");
                return;
            }

            foreach (var student in filtered)
                Console.WriteLine(student);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in FilterStudents: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ViewStatistics(StudentService service)
    {
        try
        {
            var stats = service.GetStatistics();
            if (stats.TotalStudents == 0)
            {
                Console.WriteLine("\nNo students available for statistics.");
                return;
            }

            Console.WriteLine(stats.ToString());
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in ViewStatistics: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void EnhancedSearch(StudentService service)
    {
        try
        {
            Console.WriteLine("\n--- Enhanced Search ---");
            Console.WriteLine("1. Search by Name (partial match)");
            Console.WriteLine("2. Search by Department (partial match)");
            Console.Write("Choose search option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            List<Student> results;
            switch (choice)
            {
                case 1:
                    Console.Write("Enter name to search: ");
                    string name = Console.ReadLine();
                    results = service.SearchByName(name);
                    Console.WriteLine($"\n--- Search Results for '{name}' ---");
                    break;
                case 2:
                    Console.Write("Enter department to search: ");
                    string dept = Console.ReadLine();
                    results = service.SearchByDepartment(dept);
                    Console.WriteLine($"\n--- Search Results for '{dept}' ---");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            if (results.Count == 0)
            {
                Console.WriteLine("No students found matching your search.");
                return;
            }

            Console.WriteLine($"Found {results.Count} student(s):");
            foreach (var student in results)
                Console.WriteLine(student);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in EnhancedSearch: {ex.Message}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}