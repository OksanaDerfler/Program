using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using System.Data.SqlClient;

namespace DAL
{
   public class Likerec:ILike
    {
        public bool IsLike(int Id, string Nick)
        {
            using (SqlConnection connection = new SqlConnection(Recordd.conStr))
            {
                SqlCommand command = new SqlCommand("Select * from Likerec where id=@Id and Nick=@Nick", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Nick", Nick);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        var boollike = (int)reader["Likebool"];
                        if (boollike == 0)
                        {
                            return false;
                        }
                        else { return true; }

                    }
                }

                else
                { 
                //Создаем запись
                    new Likerec().Create(Id, Nick);
                    return false;

                }


            }

            return false;
        }

        public bool Create(int Id, string Nick)
        {
            using (SqlConnection connection = new SqlConnection(Recordd.conStr))
            {
                    SqlCommand command = new SqlCommand(@"Insert into likerec (Id, Nick, Likebool)
                 values (@Id, @Nick, @Likebool)", connection);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Nick", Nick);
                    command.Parameters.AddWithValue("@Likebool", 0);
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                
            }
        }

        public bool ChangeFlag(int Id, string Nick)
        {
            int boollike=60;
            using (SqlConnection connection = new SqlConnection(DAL.Recordd.conStr))
            {
                SqlCommand command = new SqlCommand("Select * from Likerec where id=@Id and Nick=@Nick", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Nick", Nick);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        boollike = (int)reader["Likebool"];
                    }
                }
            }



            using (SqlConnection connection = new SqlConnection(DAL.Recordd.conStr))
            {
                if (boollike == 0)
                {
                    boollike = 1;
                }
                else
                {
                    boollike = 0;
                }

                SqlCommand command = new SqlCommand(@"Update likerec set likebool=@boollike where Id=@Id and Nick=@Nick", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Nick", Nick);
                command.Parameters.AddWithValue("@boollike", boollike);
                connection.Open();
                return command.ExecuteNonQuery() == 1;                
            }
        }
    }
}
