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
                     id = (int)reader["Id"],
                     title = (string)reader["Title"],
                     text = (string)reader["Text"],
                     dateStart = (DateTime)reader["DateStart"],
                     tag = (string)reader["Tag"],
                     like = (int)reader["Like"],
                     nick = (string)reader["Nick"],
                     picture = (object)reader["Picture"]
                 };
                }
                
            }
            throw new NotImplementedException();
            // throw new NotImplementedException();
        }
        
        public Record Get(int id)
        {
            Entities.Record onenn = new Entities.Record();
            return onenn;
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
