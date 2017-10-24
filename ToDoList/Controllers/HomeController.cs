using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Categories()
        {
            List<Category> model = Category.GetAll();
            return View(model);
        }

        [HttpPost("/category/new")]
        public ActionResult AddCategory()
        {
            string categoryName = Request.Form["category-name"];
            Category category = new Category(categoryName);
            category.Save();
            List<Category> model = Category.GetAll();
            return View("Categories", model);
        }

        [HttpPost("/categories/clear")]
        public ActionResult ClearCategories()
        {
            Category.ClearAll();
            List<Category> model = Category.GetAll();
            return View("Categories", model);
        }

        [HttpGet("/category/{id}")]
        public ActionResult CategoryDetails(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            model.Add("selected-task", null);
            Category selectedCategory = Category.Find(id);
            model.Add("this-category", selectedCategory);
            List<Task> categoryTasks = selectedCategory.GetTasks();
            model.Add("category-tasks", categoryTasks);
            return View("CategoryTasks", model);
        }

        [HttpPost("/category/{categoryId}/task/new")]
        public ActionResult AddTask(int categoryId)
        {
            // add a task
            string taskName = Request.Form["task-name"];
            string taskDescription = Request.Form["task-description"];
            Task task = new Task(taskName, taskDescription, categoryId);
            task.Save();

            Dictionary<string, object> model = new Dictionary<string, object> {};
            model.Add("selected-task", null);
            Category selectedCategory = Category.Find(categoryId);
            model.Add("this-category", selectedCategory);
            List<Task> categoryTasks = selectedCategory.GetTasks();
            model.Add("category-tasks", categoryTasks);
            return View("CategoryTasks", model);
        }

        [HttpGet("/category/{categoryId}/task/{taskId}")]
        public ActionResult TaskDetails(int categoryId, int taskId)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Task selectedTask = Task.Find(taskId);
            model.Add("selected-task", selectedTask);
            Category selectedCategory = Category.Find(categoryId);
            model.Add("this-category", selectedCategory);
            List<Task> categoryTasks = selectedCategory.GetTasks();
            model.Add("category-tasks", categoryTasks);
            return View("CategoryTasks", model);
        }

        [HttpPost("/category/{id}/clear")]
        public ActionResult ClearCategoryTasks(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            model.Add("selected-task", null);
            Category selectedCategory = Category.Find(id);
            model.Add("this-category", selectedCategory);
            selectedCategory.ClearTasks();
            List<Task> categoryTasks = selectedCategory.GetTasks();
            model.Add("category-tasks", categoryTasks);
            return View("CategoryTasks", model);
        }
    }
}
