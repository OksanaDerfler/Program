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
        bool Update(int id);
        bool Delete(int id);
    }


    public interface IUserr
    {
        IEnumerable<Entities.Userr> GetAllUsers();
    }
}
