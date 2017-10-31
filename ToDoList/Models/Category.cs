using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Category
    {
        public string Name {get; private set;}
        public int Id {get; private set;}

        public Category(string name, int id = 0)
        {
            Name = name;
            Id = id;
        }

        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_tasks WHERE category_id = @CategoryId;", conn);
          MySqlParameter categoryIdParameter = new MySqlParameter();
          categoryIdParameter.ParameterName = "@CategoryId";
          categoryIdParameter.Value = this.Id;

          cmd.Parameters.Add(categoryIdParameter);
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
            cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = Name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM categories;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
              int categoryId = rdr.GetInt32(0);
              string categoryName = rdr.GetString(1);
              Category newCategory = new Category(categoryName, categoryId);
              allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return allCategories;
        }

        public void AddTask(Task newTask)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO categories_tasks (category_id, task_id) VALUES (@CategoryId, @TaskId);";

          MySqlParameter category_id = new MySqlParameter();
          category_id.ParameterName = "@CategoryId";
          category_id.Value = Id;
          cmd.Parameters.Add(category_id);

          MySqlParameter task_id = new MySqlParameter();
          task_id.ParameterName = "@TaskId";
          task_id.Value = newTask.Id;
          cmd.Parameters.Add(task_id);

          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }

        public List<Task> GetTasks()
        {
          MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT tasks.* FROM categories
                JOIN categories_tasks ON (categories.id = categories_tasks.category_id)
                JOIN tasks ON (categories_tasks.task_id = tasks.id)
                WHERE categories.id = @CategoryId;";

            MySqlParameter categoryIdParameter = new MySqlParameter();
            categoryIdParameter.ParameterName = "@CategoryId";
            categoryIdParameter.Value = Id;
            cmd.Parameters.Add(categoryIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Task> tasks = new List<Task>{};

            while(rdr.Read())
            {
              int taskId = rdr.GetInt32(0);
              string taskName = rdr.GetString(1);
              string taskDescription = rdr.GetString(2);
              Task newTask = new Task(taskName, taskDescription, taskId);
              tasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return tasks;
        }

        public void ClearTasks()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks WHERE category_id = @CategoryId;";

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@CategoryId";
            categoryId.Value = Id;
            cmd.Parameters.Add(categoryId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Category Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText =  @"SELECT * FROM categories WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = searchId;
            cmd.Parameters.Add(thisId);

            int categoryId = 0;
            string categoryName = "";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                categoryId = rdr.GetInt32(0);
                categoryName = rdr.GetString(1);
            }

            Category foundCategory = new Category(categoryName, categoryId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundCategory;
        }


        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool idEquality = (this.Id == newCategory.Id);
                bool nameEquality = (this.Name == newCategory.Name);
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
