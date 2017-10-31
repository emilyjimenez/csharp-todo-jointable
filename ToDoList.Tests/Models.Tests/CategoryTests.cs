using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Models.Tests
{
    [TestClass]
    public class CategoryTests : IDisposable
    {
        public CategoryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }
        public void Dispose()
        {
          Category.ClearAll();
          Task.ClearAll();
        }

        [TestMethod]
        public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
        {
          //Arrange
          Task testTask = new Task("mow the lawn","exactly 3 inches");
          testTask.Save();

          string testName = "Home stuff";
          Category testCategory = new Category(testName);
          testCategory.Save();

          //Act
          testCategory.AddTask(testTask);
          testCategory.Delete();

          List<Category> resultTaskCategories = testTask.GetCategories();
          List<Category> testTaskCategories = new List<Category> {};

          //Assert
          CollectionAssert.AreEqual(testTaskCategories, resultTaskCategories);
        }

        [TestMethod]
        public void GetAll_CategoriesEmptyAtFirst_0()
        {
            int result = Category.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_ReturnsTrueForSameName_Category()
        {
            Category firstCategory = new Category("Household chores");
            Category secondCategory = new Category("Household chores");

            Assert.AreEqual(firstCategory, secondCategory);
        }
        [TestMethod]
        public void Save_SavesCategoryToDatabase_CategoryList()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            List<Category> result = Category.GetAll();
            List<Category> testList = new List<Category>{testCategory};

            CollectionAssert.AreEqual(testList, result);
        }
        [TestMethod]
        public void Save_DatabaseAssignsIdToCategory_Id()
        {
           Category testCategory = new Category("Household chores");
           testCategory.Save();

           Category savedCategory = Category.GetAll()[0];

           int result = savedCategory.Id;
           int testId = testCategory.Id;

           Assert.AreEqual(testId, result);
        }
        [TestMethod]
        public void Save_AssignsIdToMultipleObjects_Ids()
        {
            Category category1 = new Category("Household chores");
            category1.Save();
            Category category2 = new Category("Non-household chores");
            category2.Save();

            List<Category> allCategories = Category.GetAll();

            Assert.AreEqual(allCategories[0].Id, category1.Id);
            Assert.AreEqual(allCategories[1].Id, category2.Id);
        }
        [TestMethod]
        public void Find_FindsCategoryInDatabase_Category()
        {
            Category testCategory = new Category("Household chores");
            testCategory.Save();

            Category foundCategory = Category.Find(testCategory.Id);

            Assert.AreEqual(testCategory, foundCategory);
        }

        [TestMethod]
        public void Test_AddTask_AddsTaskToCategory()
        {
          //Arrange
          Category testCategory = new Category("Household chores");
          testCategory.Save();

          Task testTask = new Task("Mow the lawn", "stuff");
          testTask.Save();

          Task testTask2 = new Task("Water the garden", "stuff");
          testTask2.Save();

          //Act
          testCategory.AddTask(testTask);
          testCategory.AddTask(testTask2);

          List<Task> result = testCategory.GetTasks();
          List<Task> testList = new List<Task>{testTask, testTask2};

          //Assert
          CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetTasks_ReturnsAllCategoryTasks_TaskList()
        {
          //Arrange
          Category testCategory = new Category("Household chores");
          testCategory.Save();

          Task testTask1 = new Task("Mow the lawn", "stuff");
          testTask1.Save();

          Task testTask2 = new Task("Buy plane ticket", " ");
          testTask2.Save();

          //Act
          testCategory.AddTask(testTask1);
          List<Task> savedTasks = testCategory.GetTasks();
          List<Task> testList = new List<Task> {testTask1};

          //Assert
          CollectionAssert.AreEqual(testList, savedTasks);
        }
    }
}
