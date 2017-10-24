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
          Task.ClearAll();
          Category.ClearAll();
        }

        [TestMethod]
        public void GetAll_CategoriesEmptyAtFirst_0()
        {
            int result = Category.GetAll().Count;

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTasks_RetrievesAllTasksWithCategory_TaskList()
        {
          Category testCategory = new Category("Household chores");
          testCategory.Save();
          Task firstTask = new Task("Mow the lawn", "3 cm exactly",  testCategory.Id);
          firstTask.Save();
          Task secondTask = new Task("Do the dishes", "dont forget the pans",  testCategory.Id);
          secondTask.Save();

          List<Task> testTaskList = new List<Task> {firstTask, secondTask};
          List<Task> resultTaskList = testCategory.GetTasks();
          CollectionAssert.AreEqual(testTaskList, resultTaskList);
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

    }
}
