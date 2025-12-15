using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public List<int> Marks { get; set; }

        public Student(int id, string name, int age, string department, List<int> marks)
        {
            Id = id;
            Name = name;
            Age = age;
            Department = department;
            Marks = marks ?? new List<int>();
        }

        // Parameterless constructor for JSON deserialization
        public Student()
        {
            Marks = new List<int>();
        }

        public double GetAverageMarks()
        {
            return Marks.Count == 0 ? 0 : Marks.Average();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Dept: {Department}, Avg Marks: {GetAverageMarks():F2}";
        }
    }
}