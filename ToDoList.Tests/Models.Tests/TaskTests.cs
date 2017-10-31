using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System;
using System.Collections.Generic;

namespace ToDoList.Models.Tests
{
    [TestClass]
    public class TaskTests : IDisposable
    {
        public TaskTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }
        public void Dispose()
        {
            Task.ClearAll();
            Category.ClearAll();
        }
        [TestMethod]
        public void Update_UpdateNameDescriptioninTask_Task()
        {
          Task testTask = new Task("Thing", "otherthing");
          testTask.Save();

          string newName = "Different thing";
          string newDescription = "Different otherthing";

          Task newTask = new Task(newName, newDescription);
          testTask.Update(newName, newDescription);

          Assert.AreEqual(newTask.Name, testTask.Name);
          Assert.AreEqual(newTask.Description, testTask.Description);
        }

        [TestMethod]
          public void GetCategories_ReturnsAllTaskCategories_CategoryList()
          {
            //Arrange
            Task testTask = new Task("Mow the lawn", "describe");
            testTask.Save();

            Category testCategory1 = new Category("Home stuff");
            testCategory1.Save();

            Category testCategory2 = new Category("Work stuff");
            testCategory2.Save();

            //Act
            testTask.AddCategory(testCategory1);
            List<Category> result = testTask.GetCategories();
            List<Category> testList = new List<Category> {testCategory1};

            //Assert
            CollectionAssert.AreEqual(testList, result);
          }

        [TestMethod]
         public void AddCategory_AddsCategoryToTask_CategoryList()
         {
           //Arrange
           Task testTask = new Task("Mow the lawn", "stuff");
           testTask.Save();

           Category testCategory = new Category("Home stuff");
           testCategory.Save();

           //Act
           testTask.AddCategory(testCategory);

           List<Category> result = testTask.GetCategories();
           List<Category> testList = new List<Category>{testCategory};

           //Assert
           CollectionAssert.AreEqual(testList, result);
         }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            int result = Task.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Task()
        {
            Task firstTask = new Task("Mow the lawn", "Make sure you mow the lawn");
            Task secondTask = new Task("Mow the lawn", "Make sure you mow the lawn");

            Assert.AreEqual(firstTask, secondTask);
        }

        [TestMethod]
        public void Save_SavesToDatabase_TaskList()
        {
            // Arrange
            Task testTask = new Task("Mow the lawn", "3 cm exactly");

            // Act
            testTask.Save();
            List<Task> result = Task.GetAll();
            List<Task> testList = new List<Task>{testTask};

            // Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_Id()
        {
            Task testTask = new Task("Mow the lawn", "3 cm exactly");
            testTask.Save();

            Task savedTask = Task.GetAll()[0];

            int result = savedTask.Id;
            int testId = testTask.Id;

            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Save_AssignsIdToMultipleObjects_Ids()
        {
            Task task1 = new Task("Mow the lawn", "3 cm exactly");
            task1.Save();
            Task task2 = new Task("Water the plants", "but not too much");
            task2.Save();

            List<Task> allTasks = Task.GetAll();

            Assert.AreEqual(allTasks[0].Id, task1.Id);
            Assert.AreEqual(allTasks[1].Id, task2.Id);
        }

        [TestMethod]
        public void Find_FindsTaskInDatabase_Task()
        {
            Task testTask = new Task("Mow the lawn", "3 cm exactly");
            testTask.Save();

            Task foundTask = Task.Find(testTask.Id);

            Assert.AreEqual(testTask, foundTask);
        }

        [TestMethod]
        public void Delete_DeletesTaskAssociationsFromDatabase_TaskList()
        {
          //Arrange
          Category testCategory = new Category("Home stuff");
          testCategory.Save();

          string testName = "Mow the lawn";
          string testDescription = "stuff";
          Task testTask = new Task(testName, testDescription);
          testTask.Save();

          //Act
          testTask.AddCategory(testCategory);
          testTask.Delete();

          List<Task> resultCategoryTasks = testCategory.GetTasks();
          List<Task> testCategoryTasks = new List<Task> {};

          //Assert
          CollectionAssert.AreEqual(testCategoryTasks, resultCategoryTasks);
        }
    }
}
