Student Record Manager - Phase 2 

 Phase 2

 Core Enhancements Implemented

1. Persistent Storage
   - JSON-based file storage (`students.json`)
   - Automatic save on all data modifications
   - Automatic load on application startup
   - Graceful error handling for file operations

2. Update and Delete Operations
   - Full update capability with field-by-field modification
   - Safe delete with confirmation prompt
   - Validation on all operations

3. Sorting and Filtering
   - Sort by: Name, Age, Average Marks
   - Filter by: Department, Minimum Average Marks
   - Case-insensitive operations

4. Statistics and Aggregations
   - Overall class average
   - Highest and lowest averages
   - Department-wise statistics (count and average)
   - Total student count

5. Input Validation
   - Centralized validation service
   - Comprehensive validation rules:
     - ID: Must be positive
     - Name: 2+ characters, non-empty
     - Age: Between 15-100
     - Marks: Between 0-100
     - Department: Non-empty
   - Clear error messages

6. Exception Handling and Logging 
   - Structured logging to console and file (`student_management.log`)
   - Try-catch blocks throughout application
   - Graceful failure without crashes
   - Log levels: INFO, WARNING, ERROR

7. Code Structure Improvements
   - Separation of concerns with dedicated services
   - `StorageService`: Handles all file I/O
   - `ValidationService`: Centralized validation logic
   - `Logger`: Structured logging system
   - Improved encapsulation and immutability

8. Enhanced Search Capabilities
   - Partial name search (case-insensitive)
   - Partial department search (case-insensitive)
   - Returns multiple matches


 Project Structure

```
StudentManagement/
├── Student.cs                    # Student model with properties
├── StudentService.cs             # Core business logic 
├── StorageService.cs             # File persistence operations
├── ValidationService.cs          # Centralized validation
├── Logger.cs                     # Logging infrastructure
├── Program.cs                    # Console UI 
├── StudentServiceTests.cs        # Optional unit tests
├── students.json                 # Data storage 
├── student_management.log        # Application log 
└── README.md                     # This file
```


Usage Guide

Console Application Menu

```

Student Management System       

1.  Add Student
2.  View All Students
3.  Search Student by ID
4.  View Topper
5.  Update Student
6.  Delete Student
7.  Sort Students
8.  Filter Students
9.  View Statistics
10. Enhanced Search
11. Exit
```

Sample Operations

 Adding a Student
```
Choose an option: 1
Enter ID: 101
Enter Name: Alice Johnson
Enter Age: 20
Enter Department: Computer Science
Enter marks (comma-separated): 85,90,88,92
Student added successfully.
```

 Viewing Statistics
```
Choose an option: 9

 Class Statistics 
Total Students: 5
Overall Average: 82.40
Highest Average: 91.25
Lowest Average: 72.50

Department-wise Statistics:
  Computer Science: 3 students, Avg: 85.67
  Electrical Engineering: 2 students, Avg: 76.50
```

Enhanced Search
```
Choose an option: 10
1. Search by Name (partial match)
2. Search by Department (partial match)
Choose search option: 1
Enter name to search: John

--- Search Results for 'John' ---
Found 2 student(s):
ID: 101, Name: John Doe, Age: 21, Dept: Computer Science, Avg Marks: 87.50
ID: 105, Name: Johnny Smith, Age: 19, Dept: Mechanical, Avg Marks: 82.33
```
















