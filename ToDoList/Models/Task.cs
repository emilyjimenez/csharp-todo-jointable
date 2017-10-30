using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Task
    {
        public string Name {get; private set;}
        public string Description {get; private set;}
        public int Id {get; private set;}

        public Task (string name, string description, int id = 0)
        {
            Name = name;
            Description = description;
            Id = id;
        }
        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          MySqlCommand cmd = new MySqlCommand("DELETE FROM tasks WHERE id = @TaskId; DELETE FROM categories_tasks WHERE task_id = @TaskId;", conn);
          MySqlParameter taskIdParameter = new MySqlParameter();
          taskIdParameter.ParameterName = "@TaskId";
          taskIdParameter.Value = this.Id;

          cmd.Parameters.Add(taskIdParameter);
          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

        public void AddCategory(Category newCategory)
        {
          MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = newCategory.Id;
            cmd.Parameters.Add(category_id);

            MySqlParameter task_id = new MySqlParameter();
            task_id.ParameterName = "@TaskId";
            task_id.Value = Id;
            cmd.Parameters.Add(task_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Category> GetCategories()
        {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT category_id FROM categories_tasks WHERE task_id = @taskId;";

           MySqlParameter taskIdParameter = new MySqlParameter();
           taskIdParameter.ParameterName = "@taskId";
           taskIdParameter.Value = Id;
           cmd.Parameters.Add(taskIdParameter);

           var rdr = cmd.ExecuteReader() as MySqlDataReader;

           List<int> categoryIds = new List<int> {};
           while(rdr.Read())
           {
             int categoryId = rdr.GetInt32(0);
             categoryIds.Add(categoryId);
           }
           rdr.Dispose();

           List<Category> categories = new List<Category> {};
           foreach (int categoryId in categoryIds)
           {
             var categoryQuery = conn.CreateCommand() as MySqlCommand;
             categoryQuery.CommandText = @"SELECT * FROM categories WHERE id = @CategoryId;";

             MySqlParameter categoryIdParameter = new MySqlParameter();
             categoryIdParameter.ParameterName = "@CategoryId";
             categoryIdParameter.Value = categoryId;
             categoryQuery.Parameters.Add(categoryIdParameter);

             var categoryQueryRdr = categoryQuery.ExecuteReader() as MySqlDataReader;
             while(categoryQueryRdr.Read())
               {
                 int thisCategoryId = categoryQueryRdr.GetInt32(0);
                 string categoryName = categoryQueryRdr.GetString(1);
                 Category foundCategory = new Category(categoryName, thisCategoryId);
                 categories.Add(foundCategory);
               }
               categoryQueryRdr.Dispose();
           }
           conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return categories;
        }

        public static List<Task> GetAll()
        {
            List<Task> allTasks = new List<Task> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
              int taskId = rdr.GetInt32(0);
              string taskName = rdr.GetString(1);
              string taskDescription = rdr.GetString(2);
              Task newTask = new Task(taskName, taskDescription,  taskId);
              allTasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return allTasks;
        }
        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Save()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO tasks (name, description) VALUES (@TaskName, @TaskDescription);";

          MySqlParameter name = new MySqlParameter();
          name.ParameterName = "@TaskName";
          name.Value = this.Name;
          cmd.Parameters.Add(name);

          MySqlParameter description = new MySqlParameter();
          description.ParameterName = "@TaskDescription";
          description.Value = this.Description;
          cmd.Parameters.Add(description);

          cmd.ExecuteNonQuery();
          Id = (int)cmd.LastInsertedId;

          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }
        public static Task Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText =  @"SELECT * FROM `tasks` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = searchId;

            cmd.Parameters.Add(thisId);

            int taskId = 0;
            string taskName = "";
            string taskDescription = "";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                taskId = rdr.GetInt32(0);
                taskName = rdr.GetString(1);
                taskDescription = rdr.GetString(2);
            }

            Task foundTask = new Task(taskName, taskDescription, taskId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundTask;
        }

        public override bool Equals(System.Object otherTask)
        {
            if (!(otherTask is Task))
            {
                return false;
            }
            else
            {
                Task newTask = (Task) otherTask;
                bool idEquality = (this.Id == newTask.Id);
                bool nameEquality = (this.Name == newTask.Name);
                bool descriptionEquality = (this.Description == newTask.Description);
                return (idEquality && nameEquality && descriptionEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
