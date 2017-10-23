using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private Dictionary<string, Object> _model = new Dictionary<string, Object>(){};
        public HomeController()
        {
            _model.Add("TaskList", null);
            _model.Add("SelectedTask", null);
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            _model["TaskList"] = Task.GetAll();
            return View(_model);
        }

        [HttpPost("/task/new"), ActionName("Index")]
        public ActionResult IndexPost()
        {
            if (Request.Form["task-name"] != "" && Request.Form["task-description"] != "")
            {
                Task newTask = new Task(Request.Form["task-name"], Request.Form["task-description"]);
            }
            _model["TaskList"] = Task.GetAll();
            return View(_model);
        }

        [HttpPost("/task/clear"), ActionName("Index")]
        public ActionResult TaskListClear()
        {
            Task.ClearAll();
            _model["TaskList"] = Task.GetAll();
            return View(_model);
        }

        [HttpGet("/tasks/{id}"), ActionName("Index")]
        public ActionResult TaskDetail(int id)
        {
            Task task = Task.Find(id);
            _model["TaskList"] = Task.GetAll();
            _model["SelectedTask"] = task;
            return View(_model);
        }
    }
}
