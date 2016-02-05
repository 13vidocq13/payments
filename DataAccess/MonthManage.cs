using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class MonthManage
    {
        [STAThread]
        static void Main()
        {
        }

        public IList<Month> GetMonth()
        {
            using (var context = new PaymentWFEntities())
            {
                return (from q in context.Month
                        select q).ToList();
            }
        }

        public Month GetMonth(int monthId)
        {
            using (var context = new PaymentWFEntities())
            {
                return (from q in context.Month
                        where q.Id == monthId
                        select q).FirstOrDefault();
            }
        }
    }
}
