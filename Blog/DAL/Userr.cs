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

        public bool CreateUser(Entities.Userr userr)
        {

          var allusers =  new DAL.Userr().GetAllUsers();
            try{
          var tekuser = allusers.Where(p => p.UserName == userr.UserName).FirstOrDefault();
          if (tekuser.Id > 0)
          {
              throw new NotImplementedException();
          }
            }
            catch { goto outt; }



          using (SqlConnection connection = new SqlConnection(DAL.Recordd.conStr))
            {
                SqlCommand command = new SqlCommand(@"Insert into userauth (UserName, Password)
                 values (@UserName, @Password)", connection);
                command.Parameters.AddWithValue("@UserName", userr.UserName);
                command.Parameters.AddWithValue("@Password", userr.Password);                
                connection.Open();
                return command.ExecuteNonQuery() == 1;
                }

            outt:;
            return false;
        }

    }
}
