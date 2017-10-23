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
    }
}
