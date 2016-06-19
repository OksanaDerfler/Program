using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Entities;
using System.Data.SqlClient;


namespace DAL
{
    public class Recordd:IRecord
    {
        public const string conStr = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=d:\Distrib\Develop\DerfLer\Blog\Blog\App_Data\aspnet-Blog-20160526232425.mdf;Integrated Security=True";
        
        public IEnumerable<Entities.Record> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {               
                SqlCommand command = new SqlCommand("Select * from records",connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                 yield return new Record()
                 { 
                     Id = (int)reader["Id"],
                     Title = (string)reader["Title"],
                     Text = (string)reader["Text"],
                     DateStart = (DateTime)reader["DateStart"],
                     Tag = (string)reader["Tag"],
                     Like = (int)reader["Like"],
                     Nick = (string)reader["Nick"],
                     Picture = (object)reader["Picture"]
                 };
                }
                
            }
        }
        
        public Record Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlCommand command = new SqlCommand("Select * from records where Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new Record()
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        Text = (string)reader["Text"],
                        DateStart = (DateTime)reader["DateStart"],
                        Tag = (string)reader["Tag"],
                        Like = (int)reader["Like"],
                        Nick = (string)reader["Nick"],
                        Picture = (object)reader["Picture"]
                    };
                }
               return new Record();
            } 
        }

        public bool Create(Entities.Record record)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                if (record.Picture == null)
                {
                    SqlCommand command = new SqlCommand(@"Insert into records (Title, Text, DateStart, Tag, [Like], Nick )
                 values (@Title, @Text, @DateStart, @Tag, @Like, @Nick)", connection);
                    command.Parameters.AddWithValue("@Title", record.Title);
                    command.Parameters.AddWithValue("@Text", record.Text);
                    command.Parameters.AddWithValue("@DateStart", record.DateStart);
                    command.Parameters.AddWithValue("@Tag", record.Tag);
                    command.Parameters.AddWithValue("@Like", record.Like);
                    command.Parameters.AddWithValue("@Nick", record.Nick);                    
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                else
                {
                    SqlCommand command = new SqlCommand(@"Insert into records (Title, Text, DateStart, Tag, Nick, [Like], Picture)
                 values (@Title, @Text, @DateStart, @Tag, @Nick, @Like, @Picture)", connection);
                    command.Parameters.AddWithValue("@Title", record.Title);
                    command.Parameters.AddWithValue("@Text", record.Text);
                    command.Parameters.AddWithValue("@DateStart", record.DateStart);
                    command.Parameters.AddWithValue("@Tag", record.Tag);
                    command.Parameters.AddWithValue("@Nick", record.Nick);
                    command.Parameters.AddWithValue("@Like", record.Like);
                    command.Parameters.AddWithValue("@Picture", record.Picture);
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }

            }
        }

        public bool Update(int id)
        {
            return false;
        }

        public bool Delete(int id)
        {
            return false;
        }

        public IEnumerable<Record> Search (string searTag)
        { 
          using (SqlConnection connection = new SqlConnection(conStr))
            {
                SqlCommand command = new SqlCommand("Select * from records where tag like '%спорт%'", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                 yield return new Record()
                 { 
                     Id = (int)reader["Id"],
                     Title = (string)reader["Title"],
                     Text = (string)reader["Text"],
                     DateStart = (DateTime)reader["DateStart"],
                     Tag = (string)reader["Tag"],
                     Like = (int)reader["Like"],
                     Nick = (string)reader["Nick"],
                     Picture = (object)reader["Picture"]
                 };
                }
                
            }
        }

    }
}
