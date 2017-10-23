using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System;

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
            //Task.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            int result = Task.GetAll().Count;

            Assert.AreEqual(0, result);
        }
    }
}
