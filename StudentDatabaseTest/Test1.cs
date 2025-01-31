using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StudentDatabase;


namespace StudentDatabaseTest
{
    [TestClass]
    [DoNotParallelize]
    public sealed class StudentTableTests
    {
        [TestMethod]
        public void AddGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();

            studentTable.AddStudent(12, "John", 23, "Computer Science");
            Student student = studentTable.GetStudent(12);

            Assert.AreEqual(student.getName(), "John");
            Assert.AreEqual(student.getAge(), 23);
            Assert.AreEqual(student.getDegree(), "Computer Science");
        }

        [TestMethod]
        public void AddNonGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();

            studentTable.AddStudent(12, "John", 23, null);
            Student student = studentTable.GetStudent(12);

            Assert.AreEqual(student.getName(), "John");
            Assert.AreEqual(student.getAge(), 23);
            Assert.AreEqual(student.getDegree(), null);
        }

        [TestMethod]
        public void ViewGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(15, "Jeremy", 27, "Computer Science");

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                studentTable.ViewStudent(15);

                string expectedOutput = "ID: 15, Name: Jeremy, Age: 27\r\nDegree: Computer Science\r\n";
                Assert.AreEqual(expectedOutput, sw.ToString());
            }
        }

        [TestMethod]
        public void ViewNonGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, null);
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                studentTable.ViewStudent(12);

                string expectedOutput = "ID: 12, Name: John, Age: 23\r\n";
                Assert.AreEqual(expectedOutput, sw.ToString());
            }
        }

        [TestMethod]
        public void UpdateGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, "Computer Science");

            studentTable.UpdateStudent(12, "Jeremy", 27, "Mechanical Engineering");
            Student student = studentTable.GetStudent(12);

            Assert.AreEqual(student.getName(), "Jeremy");
            Assert.AreEqual(student.getAge(), 27);
            Assert.AreEqual(student.getDegree(), "Mechanical Engineering");

        }

        [TestMethod]
        public void UpdateNonGraduateStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, null);

            studentTable.UpdateStudent(12, "Jeremy", 27, "Mechanical Engineering");
            Student student = studentTable.GetStudent(12);

            Assert.AreEqual(student.getName(), "Jeremy");
            Assert.AreEqual(student.getAge(), 27);
            Assert.AreEqual(student.getDegree(), "Mechanical Engineering");

        }

        [TestMethod]
        public void DeleteStudent()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, "Computer Science");
            studentTable.DeleteStudent(12);
            Student student = studentTable.GetStudent(12);
            Assert.AreEqual(student, null);
        }
    }

    [TestClass]
    [DoNotParallelize]
    public sealed class ValidateInputTests 
    {
        [TestMethod]
        public void SuccessfulValidateID()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateID("12"), 12);
        }

        [TestMethod]
        public void FailedValidateID()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);

            using (StringWriter sw = new StringWriter())
            // Valid ID value to input after the invalid input, therefore the program will not loop again.
            using (StringReader sr = new StringReader("12\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                validateInput.ValidateID("12a");

                string expectedOutput = "Invalid input! Please input a valid ID: ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }

        [TestMethod]
        public void SuccessfulValidateStudentByID()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, "Computer Science");
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateStudentByID(12), true);
        }

        [TestMethod]
        public void FailedValidateStudentByID()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, "Computer Science");
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateStudentByID(5), false);
        }

        [TestMethod]
        public void SuccessfulValidateStudents()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            studentTable.AddStudent(12, "John", 23, "Computer Science");
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateStudents(), true);
        }

        [TestMethod]
        public void FailedValidateStudents()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateStudents(), false);
        }

        [TestMethod]
        public void SuccessfulValidateAge()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateAge("23"), 23);
        }

        [TestMethod]
        public void FailedValidateAge()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);

            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("20\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                validateInput.ValidateAge("a");

                string expectedOutput = "Invalid input! Please input a valid age (between 16 and 99): ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }

        [TestMethod]
        public void FailedValidateAgeValidNumber()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);

            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("20\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                validateInput.ValidateAge("10");

                string expectedOutput = "Invalid input! Please input a valid age (between 16 and 99): ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }

        [TestMethod]
        public void SuccessfulValidateOption()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateOption("3"), "3");
        }

        [TestMethod]
        public void FailedValidateOption() 
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);

            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("2\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                validateInput.ValidateOption("8a");

                string expectedOutput = "Invalid input! Please input a valid option (1/2/3/4/5): ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }

        [TestMethod]
        public void SuccessfulValidateGraduate()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateGraduate("y"), true);
        }

        [TestMethod]
        public void FailedValidateGraduate()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("n\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                validateInput.ValidateGraduate("a");
                string expectedOutput = "Invalid input! Please input a valid option (y/n): ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }

        [TestMethod]
        public void SuccessfulValidateYesOrNo()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);
            Assert.AreEqual(validateInput.ValidateYesOrNo("y"), "y");
        }

        [TestMethod]
        public void FailedValidateYesOrNo()
        {
            StudentDatabase.StudentTable studentTable = new StudentTable();
            StudentDatabase.ValidateInput validateInput = new ValidateInput(studentTable);

            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("n\n"))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);
                validateInput.ValidateYesOrNo("a");
                string expectedOutput = "Invalid input! Please input a valid option (y/n): ";
                Assert.IsTrue(sw.ToString().Contains(expectedOutput));
            }
        }
    }


}



