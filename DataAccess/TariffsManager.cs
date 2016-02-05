using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TariffsManager
    {
        public Tariffs GetTariff(int tariffId)
        {
            using (var context = new PaymentWFEntities())
            {
                return context.Tariffs.Where(x => x.Id == tariffId).ToList().LastOrDefault();
            }
        }

        public Tariffs GetCurrentTariff(int serviceId)
        {
            using (var context = new PaymentWFEntities())
            {
                return context.Tariffs.Where(x => x.ServiceId == serviceId).ToList().LastOrDefault();
            }
        }

        public IList<Tariffs> GetCurrentTariffs()
        {
            using (var context = new PaymentWFEntities())
            {
                var services = new ServiceManager().GetServices(context);

                if(services.Count == 0)
                    throw new InvalidOperationException("не найден коммунальный платеж");

                return services.Select(item => GetCurrentTariff(item.Id)).Where(res => res != null).ToList();
            }
        }

        public void SaveTariff(Tariffs tariff)
        {
            using (var context = new PaymentWFEntities())
            {
                //var data = new Tariffs
                //               {
                //                   ServiceId = tariff.ServiceId,
                //                   Price = tariff.Price,
                //                   Overexpenditure = tariff.Overexpenditure,
                //                   DateSet = DateTime.Now
                //               };
                
                context.Tariffs.Add(tariff);
                context.SaveChanges();
            }
        }
    }
}
