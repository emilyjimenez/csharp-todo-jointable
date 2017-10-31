using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/tasks")]
    public ActionResult Tasks()
    {
      List<Task> allTasks = Task.GetAll();
      return View(allTasks);
    }

    [HttpGet("/categories")]
    public ActionResult Categories()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/tasks/new")]
    public ActionResult TaskForm()
    {
      return View();
    }

    [HttpPost("/tasks/new")]
    public ActionResult TaskCreate()
    {
      Task newTask = new Task(Request.Form["task-name"], Request.Form["task-description"]);
      newTask.Save();
      return View("Success");
    }

    [HttpGet("/categories/new")]
    public ActionResult CategoryForm()
    {
      return View();
    }

    [HttpPost("/categories/new")]
    public ActionResult CategoryCreate()
    {
      Category newCategory = new Category(Request.Form["category-name"]);
      newCategory.Save();
      return View("Success");
    }

    [HttpGet("/tasks/{id}")]
    public ActionResult TaskDetail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Task selectedTask = Task.Find(id);
      List<Category> TaskCategories = selectedTask.GetCategories();
      List<Category> AllCategories = Category.GetAll();
      model.Add("task", selectedTask);
      model.Add("taskCategories", TaskCategories);
      model.Add("allCategories", AllCategories);
      return View( model);
    }

    [HttpGet("/categories/{id}")]
    public ActionResult CategoryDetail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category SelectedCategory = Category.Find(id);
      List<Task> CategoryTasks = SelectedCategory.GetTasks();
      List<Task> AllTasks = Task.GetAll();
      model.Add("category", SelectedCategory);
      model.Add("categoryTasks", CategoryTasks);
      model.Add("allTasks", AllTasks);
      return View(model);
    }

    //ADD TASK TO CATEGORY
    [HttpPost("categories/{categoryId}/tasks/new")]
    public ActionResult CategoryAddTask(int categoryId)
    {
      Category category = Category.Find(categoryId);
      Task task = Task.Find(Int32.Parse(Request.Form["task-id"]));
      category.AddTask(task);
      return View("Success");
    }
//ADD CATEGORY TO TASK
    [HttpPost("tasks/{taskId}/categories/new")]
    public ActionResult TaskAddCategory(int taskId)
    {
      Task task = Task.Find(taskId);
      Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
      task.AddCategory(category);
      return View("Success");
    }
  }
}
