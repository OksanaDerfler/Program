using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Bll
    {
        public List<string> GetDistinctTags()
        {
            var db = new DAL.Recordd().GetAll();
            return db.Select(p => p.Tag).Distinct().ToList();
        }


    }
}
