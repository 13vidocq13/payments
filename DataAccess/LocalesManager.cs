using System.Linq;

namespace DataAccess
{
    class LocalesManager
    {
        public Locales GetLocale(string locale, PaymentWFEntities context)
        {
            return context.Locales.FirstOrDefault(x => x.Locale == locale);
        }
    }
}
