using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentDatabase
{
    public abstract class AbstractStudent
    {
        protected int ID { get; set; }
        protected string Name { get; set; }
        protected int Age { get; set; }
        protected string? Degree { get; set; } //Degree is nullable as it is not a requirement for all students
    }

    // Concrete class that inherits from the abstract class, maintaining abstraction and allowing future extensibility (Open/Closed Principle and Liskov Principle)
    public class Student : AbstractStudent
    {
        public Student(int id, string name, int age, string degree)
        {
            ID = id;
            Name = name;
            Age = age;
            Degree = degree;
        }

        public int getID()
        {
            return ID;
        }

        public void setID(int id)
        {
            ID = id;
        }

        public string getName()
        {
            return Name;
        }

        public void setName(string name)
        {
            Name = name;
        }

        public int getAge()
        {
            return Age;
        }

        public void setAge(int age)
        {
            Age = age;
        }

        public string getDegree()
        {
            return Degree;
        }

        public void setDegree(string degree)
        {
            Degree = degree;
        }
    }

    public interface StudentTableActions
    {
        void AddStudent(int id, string name, int age, string degree);
        void ViewStudent(int id);
        void UpdateStudent(int id, string name, int age, string degree);
        void DeleteStudent(int id);
    }

    public class StudentTable : StudentTableActions
    {
        //Encapsulation of the Student list as it is important to keep it private and only allow access through the methods
        private List<Student> StudentList { get; set; } = new List<Student>();

        // Creates new Student object and adds it to the list
        public void AddStudent(int id, string name, int age, string degree)
        {
            try
            {
                Student newStudent = new Student(id, name, age, degree);
                StudentList.Add(newStudent);
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occurred!: ", e.Message);
            }
        }

        // View student from list
        public void ViewStudent(int id)
        {
            try
            {
                foreach (Student Student in StudentList)
                {
                    if (Student.getID() == id)
                    {
                        Console.WriteLine("ID: " + Student.getID() + ", Name: " + Student.getName() + ", Age: " + Student.getAge());
                        if (Student.getDegree() != null)
                        {
                            Console.WriteLine("Degree: " + Student.getDegree());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occurred!: ", e.Message);
            }
        }

        // Return student list
        // Using Enumerable here instead of List as it is more abstract and allows for future extensibility (as it doesn't require changing the method signature)
        // This is an example of the Interface Segregation Principle and encapsulation as it hides the type of collection being used
        // Also better for unit testing becuase there is no need to specifically use List 
        public IEnumerable<Student> GetStudentList()
        {
            return StudentList;

        }

        // Update student in list
        public void UpdateStudent(int id, string name, int age, string degree)
        {
            try
            {
                // View student from list
                foreach (Student Student in StudentList)
                {
                    if (Student.getID() == id)
                    {
                        Student.setName(name);
                        Student.setAge(age);
                        Student.setDegree(degree);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occurred!: ", e.Message);
            }
        }

        // Get student from list
        public Student GetStudent(int id)
        {
            try
            {
                foreach (Student Student in StudentList)
                {
                    if (Student.getID() == id)
                    {
                        return Student;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occurred!: ", e.Message);
                return null;
            }

        }

        // Delete Student from list
        public void DeleteStudent(int id)
        {
            try
            {
                for (int i = StudentList.Count - 1; i >= 0; i--)
                {
                    if (StudentList[i].getID() == id)
                    {
                        StudentList.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The following error occurred!: ", e.Message);
            }

        }
    }

    //Prompting the user to choose an option then calling the appropriate method
    class ProcessInput
    {
        private StudentTable StudentTable = new StudentTable();

        //Gets a valid option for waht to do from user. If user wants to exit, returns false
        public bool GetValidOption(bool continueProgram)
        {
            Console.WriteLine("_________________________________");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Student");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.WriteLine("_________________________________");
            Console.Write("Select An Option: ");
            string option = Console.ReadLine();
            ValidateInput ValidateInput = new ValidateInput(StudentTable);
            option = ValidateInput.ValidateOption(option);
            ImplementAction ImplementAction = new ImplementAction(StudentTable, ValidateInput);
            string action;
            switch (option)
            {
                case "1":
                    action = "Add"; // May not need these variables as the right method will be called anyways
                    ImplementAction.Add();
                    break;
                case "2":
                    action = "View";
                    ImplementAction.View();
                    break;
                case "3":
                    action = "Update";
                    ImplementAction.Update();
                    break;
                case "4":
                    action = "Delete";
                    ImplementAction.Delete();
                    break;
                case "5":
                    continueProgram = false;
                    break;
            }
            return continueProgram;
        }
    }

    //Implementation of the different actions
    class ImplementAction
    {
        //Must use the already created objects otherwise it won't have the same Student list.
        private StudentTable StudentTable;
        private ValidateInput ValidateInput;

        public ImplementAction(StudentTable studentTable, ValidateInput validateInput)
        {
            StudentTable = studentTable;
            ValidateInput = validateInput;
        }

        public void Add()
        {
            Console.Write("Please enter student's ID: ");
            int id = ValidateInput.ValidateID(Console.ReadLine());
            bool existingStudent = ValidateInput.ValidateStudentByID(id);
            while (existingStudent)
            {
                Console.WriteLine("Invalid input! That student ID has already been taken.");
                Console.Write("Do you want to try another ID (y/n)? ");
                string continueProgram = new ValidateInput(StudentTable).ValidateYesOrNo(Console.ReadLine());
                if (continueProgram == "n")
                {
                    return;
                }
                Console.Write("Please enter a valid student ID: ");
                id = ValidateInput.ValidateID(Console.ReadLine());
                existingStudent = ValidateInput.ValidateStudentByID(id);
            }

            Console.Write("Please enter student's name: ");
            string name = Console.ReadLine();

            Console.Write("Please enter student's age: ");
            int age = ValidateInput.ValidateAge(Console.ReadLine());

            Console.Write("Is this student a graduate (y/n)? ");
            bool graduate = ValidateInput.ValidateGraduate(Console.ReadLine());
            string degree = null;
            if (graduate)
            {
                Console.Write("Please enter student's degree: ");
                degree = Console.ReadLine();
            }
            StudentTable.AddStudent(id, name, age, degree);
        }

        public void View()
        {
            //Check if there are any students in the list
            if (ValidateInput.ValidateStudents() == false)
            {
                Console.WriteLine("There are currently no students in the database.");
                return;
            }
            //Check Student exists
            Console.Write("Please enter student's ID: ");
            int id = ValidateInput.ValidateID(Console.ReadLine());
            bool existingStudent = ValidateInput.ValidateStudentByID(id);
            while (!existingStudent)
            {
                Console.WriteLine("Invalid input! There is no student with that ID.");
                Console.Write("Do you want to try another ID (y/n)? ");
                string continueProgram = new ValidateInput(StudentTable).ValidateYesOrNo(Console.ReadLine());
                if (continueProgram == "n")
                {
                    return;
                }
                Console.Write("Please enter a valid student ID: ");
                id = ValidateInput.ValidateID(Console.ReadLine());
                existingStudent = ValidateInput.ValidateStudentByID(id);
            }
            StudentTable.ViewStudent(id);
        }

        public void Update()
        {
            //Check if there are any students in the list
            if (ValidateInput.ValidateStudents() == false)
            {
                Console.WriteLine("There are currently no students in the database.");
                return;
            }
            //Check Student exists
            Console.Write("Please enter the ID of the student you want to update: ");
            int id = ValidateInput.ValidateID(Console.ReadLine());
            bool existingStudent = ValidateInput.ValidateStudentByID(id);
            while (!existingStudent)
            {
                Console.WriteLine("Invalid input! There is no student with that ID.");
                Console.Write("Do you want to try another ID (y/n)? ");
                string continueProgram = new ValidateInput(StudentTable).ValidateYesOrNo(Console.ReadLine());
                if (continueProgram == "n")
                {
                    return;
                }
                Console.Write("Please enter a valid student ID: ");
                id = ValidateInput.ValidateID(Console.ReadLine());
                existingStudent = ValidateInput.ValidateStudentByID(id);
            }

            //Get name
            Console.Write("Please enter student's updated name: ");
            string name = Console.ReadLine();

            //Get age
            Console.Write("Please enter student's updated age: ");
            int age = ValidateInput.ValidateAge(Console.ReadLine());

            //If they are graduate get degree
            string degree = StudentTable.GetStudent(id).getDegree();
            if (degree != null)
            {
                //If they want to change the degree or leave it
                Console.Write("Do you want to change the degree (y/n)? ");
                string changeDegree = ValidateInput.ValidateYesOrNo(Console.ReadLine());
                if (changeDegree == "y")
                {
                    Console.Write("Please enter student's updated degree: ");
                    degree = Console.ReadLine();
                }

            }
            else
            {
                Console.Write("Do you want to add a degree (y/n)? ");
                string changeDegree = ValidateInput.ValidateYesOrNo(Console.ReadLine());
                if (changeDegree == "y")
                {
                    Console.Write("Please enter student's new degree: ");
                    degree = Console.ReadLine();
                }
            }
            StudentTable.UpdateStudent(id, name, age, degree);
        }

        public void Delete()
        {
            //Check if there are any students in the list
            if (ValidateInput.ValidateStudents() == false)
            {
                Console.WriteLine("There are currently no students in the database.");
                return;
            }
            //Check Student actually exists
            Console.Write("Please enter student's ID: ");
            int id = ValidateInput.ValidateID(Console.ReadLine());
            bool existingStudent = ValidateInput.ValidateStudentByID(id);
            while (!existingStudent)
            {
                Console.WriteLine("Invalid input! There is no student with that ID.");
                Console.Write("Do you want to try another ID (y/n)? ");
                string continueProgram = new ValidateInput(StudentTable).ValidateYesOrNo(Console.ReadLine());
                if (continueProgram == "n")
                {
                    return;
                }
                Console.Write("Please enter a valid student ID: ");
                id = ValidateInput.ValidateID(Console.ReadLine());
                existingStudent = ValidateInput.ValidateStudentByID(id);
            }
            StudentTable.DeleteStudent(id);
            Console.WriteLine("Student with ID (" + id + ") has been deleted.");
        }
    }

    //Ensures the inputs given are valid
    public class ValidateInput
    {
        private StudentTable StudentTable;

        public ValidateInput(StudentTable studentTable)
        {
            StudentTable = studentTable;
        }

        public int ValidateID(string input)
        {
            int id;
            while (!int.TryParse(input, out id) || id > 2000 || id < 0)
            {
                Console.Write("Invalid input! Please input a valid ID: ");
                input = Console.ReadLine();
            }
            return id;
        }

        public bool ValidateStudentByID(int id)
        {
            IEnumerable<Student> students = StudentTable.GetStudentList();
            foreach (Student student in students)
            {
                if (student.getID() == id)
                {
                    return true;
                }
            }
            return false;

        }

        //Checks if there are any students in the list
        public bool ValidateStudents()
        {
            IEnumerable<Student> students = StudentTable.GetStudentList();
            return students.Any();

        }

        public int ValidateAge(string input)
        {
            int age;
            while (!int.TryParse(input, out age) || 100 < age || age < 16)
            {
                Console.Write("Invalid input! Please input a valid age (between 16 and 99): ");
                input = Console.ReadLine();
            }
            return age;
        }

        public string ValidateOption(string option)
        {
            string[] validOptions = { "1", "2", "3", "4", "5" };
            while (!validOptions.Contains(option))
            {
                Console.Write("Invalid input! Please input a valid option (1/2/3/4/5): ");
                option = Console.ReadLine();
            }
            return option;
        }

        public bool ValidateGraduate(string graduate)
        {
            if (graduate != "y" && graduate != "n")
            {
                Console.Write("Invalid input! Please input a valid option (y/n): ");
                graduate = Console.ReadLine();
            }
            return graduate == "y";
        }

        public string ValidateYesOrNo(string continuation)
        {
            if (continuation != "y" && continuation != "n")
            {
                Console.Write("Invalid input! Please input a valid option (y/n): ");
                continuation = Console.ReadLine();
            }
            return continuation;
        }
    }

    public class Program
    {
        public static void Main()
        {
            bool continueProgram = true;
            ProcessInput ProcessInput = new ProcessInput();

            while (continueProgram)
            {
                continueProgram = ProcessInput.GetValidOption(continueProgram);
            }
        }
    }
}
