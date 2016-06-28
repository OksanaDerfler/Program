using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace Interfaces
{
    public interface IRecord
    {
        IEnumerable<Record> GetAll();
        Record Get(int id);
        bool Create(Record record);
        bool Update(Entities.Record record);
        bool Delete(int id);
        bool Like(int ID, bool flag, string name);
    }

    public interface IUserr
    {
        IEnumerable<Entities.Userr> GetAllUsers();
        bool CreateUser(Entities.Userr user);
    }

    public interface ILike
    {
        bool IsLike(int Id, string Name);
        bool Create(int Id, string Nick);
        bool ChangeFlag (int Id, string Nick);       

    }

}
