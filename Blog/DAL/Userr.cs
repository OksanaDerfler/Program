using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class Userr : IUserr
    {

        public IEnumerable<Entities.Userr> GetAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(Recordd.conStr))
            {
                SqlCommand command = new SqlCommand("Select * from userauth", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Entities.Userr()
                    {
                        Id = (int)reader["Id"],
                        UserName = (string)reader["UserName"],
                        Password = (string)reader["Password"],
                    };
                }

            }
        }
    }
}
