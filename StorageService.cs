using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace StudentManagement
{
    public class StorageService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public StorageService(string filePath = "students.json")
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public List<Student> LoadStudents()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Logger.Info("No existing data file found. Starting with empty student list.");
                    return new List<Student>();
                }

                string jsonContent = File.ReadAllText(_filePath);
                
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    Logger.Warning("Data file is empty.");
                    return new List<Student>();
                }

                var students = JsonSerializer.Deserialize<List<Student>>(jsonContent, _jsonOptions);
                Logger.Info($"Loaded {students?.Count ?? 0} students from file.");
                return students ?? new List<Student>();
            }
            catch (JsonException ex)
            {
                Logger.Error($"Failed to parse JSON data: {ex.Message}");
                return new List<Student>();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load students: {ex.Message}");
                return new List<Student>();
            }
        }

        public bool SaveStudents(List<Student> students)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(students, _jsonOptions);
                File.WriteAllText(_filePath, jsonContent);
                Logger.Info($"Saved {students.Count} students to file.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to save students: {ex.Message}");
                return false;
            }
        }
    }
}