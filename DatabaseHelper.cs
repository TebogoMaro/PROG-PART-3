using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CyberSecurityBotGUI1
{
    public class DatabaseHelper
    {
        string connectionString = "server=localhost;database=CyberTaskDB;uid=root;password=Theboymaro@17;";

        public void AddTask(TaskModel task)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    "INSERT INTO tasks (Title, Description, Reminder, Status)" +
                    "VALUES (@Title, @Description, @Reminder, @Status)";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@Reminder", task.Reminder);
                command.Parameters.AddWithValue("@Status", task.Status);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<TaskModel> GetTasks()
        {
            List<TaskModel> tasks =
                new List<TaskModel>();

            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "SELECT * FROM tasks";

            MySqlCommand command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                TaskModel task = new TaskModel();

                task.Id = Convert.ToInt32(reader["Id"]);

                task.Title = reader["Title"].ToString();

                task.Description = reader["Description"].ToString();

                task.Reminder = reader["Reminder"].ToString();

                task.Status = reader["Status"].ToString();

                tasks.Add(task);
            }

            connection.Close();

            return tasks;
        }

        public void CompleteTask(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "UPDATE tasks SET Status = 'Completed' WHERE Id = @Id";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void DeleteTask(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string query = "DELETE FROM tasks WHERE Id = @Id";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
