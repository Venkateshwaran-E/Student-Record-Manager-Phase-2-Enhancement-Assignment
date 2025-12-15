using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement
{
    public class ValidationService
    {
        public static ValidationResult ValidateStudent(Student student)
        {
            var errors = new List<string>();

            if (student.Id <= 0)
                errors.Add("Student ID must be positive.");

            if (string.IsNullOrWhiteSpace(student.Name))
                errors.Add("Name cannot be empty.");
            else if (student.Name.Length < 2)
                errors.Add("Name must be at least 2 characters long.");

            if (student.Age <= 0)
                errors.Add("Age must be positive.");
            else if (student.Age < 15 || student.Age > 100)
                errors.Add("Age must be between 15 and 100.");

            if (string.IsNullOrWhiteSpace(student.Department))
                errors.Add("Department cannot be empty.");

            if (student.Marks != null && student.Marks.Any(m => m < 0 || m > 100))
                errors.Add("Marks must be between 0 and 100.");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

        public static ValidationResult ValidateId(int id)
        {
            var errors = new List<string>();

            if (id <= 0)
                errors.Add("ID must be positive.");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

        public static ValidationResult ValidateMarks(List<int> marks)
        {
            var errors = new List<string>();

            if (marks == null || marks.Count == 0)
                errors.Add("At least one mark must be provided.");
            else if (marks.Any(m => m < 0 || m > 100))
                errors.Add("All marks must be between 0 and 100.");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public string GetErrorMessage()
        {
            return string.Join("; ", Errors);
        }
    }
}