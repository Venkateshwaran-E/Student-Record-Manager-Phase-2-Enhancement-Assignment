using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement
{
    public class StudentService
    {
        private readonly List<Student> _students;
        private readonly StorageService _storage;

        public StudentService(StorageService storage)
        {
            _storage = storage;
            _students = _storage.LoadStudents();
        }

        // Original Methods
        public bool AddStudent(Student student)
        {
            var validation = ValidationService.ValidateStudent(student);
            if (!validation.IsValid)
            {
                Logger.Warning($"Validation failed: {validation.GetErrorMessage()}");
                throw new ArgumentException(validation.GetErrorMessage());
            }

            if (_students.Any(s => s.Id == student.Id))
            {
                Logger.Warning($"Attempted to add duplicate student ID: {student.Id}");
                return false;
            }

            _students.Add(student);
            _storage.SaveStudents(_students);
            Logger.Info($"Student added: ID={student.Id}, Name={student.Name}");
            return true;
        }

        public List<Student> GetAllStudents()
        {
            return new List<Student>(_students);
        }

        public Student GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public Student GetTopper()
        {
            return _students
                .OrderByDescending(s => s.GetAverageMarks())
                .FirstOrDefault();
        }

        // Enhancement 2: Update and Delete
        public bool UpdateStudent(int id, Student updatedStudent)
        {
            var validation = ValidationService.ValidateStudent(updatedStudent);
            if (!validation.IsValid)
            {
                Logger.Warning($"Update validation failed: {validation.GetErrorMessage()}");
                throw new ArgumentException(validation.GetErrorMessage());
            }

            var existingStudent = _students.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
            {
                Logger.Warning($"Student not found for update: ID={id}");
                return false;
            }

            // Check if new ID conflicts with another student
            if (updatedStudent.Id != id && _students.Any(s => s.Id == updatedStudent.Id))
            {
                Logger.Warning($"Cannot update: ID {updatedStudent.Id} already exists");
                throw new ArgumentException($"Student ID {updatedStudent.Id} already exists.");
            }

            existingStudent.Id = updatedStudent.Id;
            existingStudent.Name = updatedStudent.Name;
            existingStudent.Age = updatedStudent.Age;
            existingStudent.Department = updatedStudent.Department;
            existingStudent.Marks = updatedStudent.Marks;

            _storage.SaveStudents(_students);
            Logger.Info($"Student updated: ID={id} -> {updatedStudent.Id}");
            return true;
        }

        public bool DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                Logger.Warning($"Student not found for deletion: ID={id}");
                return false;
            }

            _students.Remove(student);
            _storage.SaveStudents(_students);
            Logger.Info($"Student deleted: ID={id}, Name={student.Name}");
            return true;
        }

        // Enhancement 3: Sorting and Filtering
        public List<Student> GetStudentsSortedByName()
        {
            return _students.OrderBy(s => s.Name).ToList();
        }

        public List<Student> GetStudentsSortedByAge()
        {
            return _students.OrderBy(s => s.Age).ToList();
        }

        public List<Student> GetStudentsSortedByAverage()
        {
            return _students.OrderByDescending(s => s.GetAverageMarks()).ToList();
        }

        public List<Student> FilterByDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
                return new List<Student>();

            return _students
                .Where(s => s.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Student> FilterByMinimumAverage(double minAverage)
        {
            return _students
                .Where(s => s.GetAverageMarks() >= minAverage)
                .ToList();
        }

        // Enhancement 4: Statistics and Aggregations
        public Statistics GetStatistics()
        {
            if (_students.Count == 0)
                return new Statistics();

            var averages = _students.Select(s => s.GetAverageMarks()).ToList();

            return new Statistics
            {
                TotalStudents = _students.Count,
                OverallAverage = averages.Average(),
                HighestAverage = averages.Max(),
                LowestAverage = averages.Min(),
                DepartmentStats = GetDepartmentStatistics()
            };
        }

        private Dictionary<string, DepartmentStats> GetDepartmentStatistics()
        {
            return _students
                .GroupBy(s => s.Department)
                .ToDictionary(
                    g => g.Key,
                    g => new DepartmentStats
                    {
                        StudentCount = g.Count(),
                        AverageMarks = g.Average(s => s.GetAverageMarks())
                    }
                );
        }

        // Enhancement 8: Enhanced Search
        public List<Student> SearchByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Student>();

            return _students
                .Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Student> SearchByDepartment(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Student>();

            return _students
                .Where(s => s.Department.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    // Enhancement 4: Statistics Models
    public class Statistics
    {
        public int TotalStudents { get; set; }
        public double OverallAverage { get; set; }
        public double HighestAverage { get; set; }
        public double LowestAverage { get; set; }
        public Dictionary<string, DepartmentStats> DepartmentStats { get; set; }

        public Statistics()
        {
            DepartmentStats = new Dictionary<string, DepartmentStats>();
        }

        public override string ToString()
        {
            var result = $"\n=== Class Statistics ===\n";
            result += $"Total Students: {TotalStudents}\n";
            result += $"Overall Average: {OverallAverage:F2}\n";
            result += $"Highest Average: {HighestAverage:F2}\n";
            result += $"Lowest Average: {LowestAverage:F2}\n";
            
            if (DepartmentStats.Any())
            {
                result += "\nDepartment-wise Statistics:\n";
                foreach (var dept in DepartmentStats)
                {
                    result += $"  {dept.Key}: {dept.Value.StudentCount} students, Avg: {dept.Value.AverageMarks:F2}\n";
                }
            }

            return result;
        }
    }

    public class DepartmentStats
    {
        public int StudentCount { get; set; }
        public double AverageMarks { get; set; }
    }
}