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
            return false;
        }

        public bool Update(int id)
        {
            return false;
        }

        public bool Delete(int id)
        {
            return false;
        }
    }
}
