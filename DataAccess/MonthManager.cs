using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class MonthManager
    {
        [STAThread]
        static void Main()
        {
        }

        public IList<Month> GetMonths(string locale)
        {
            using (var context = new PaymentWFEntities())
            {
                var currentLocale = new LocalesManager().GetLocale(locale, context);

                return (from q in context.Month
                        where q.LocaleId == currentLocale.Id
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
