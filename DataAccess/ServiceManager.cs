using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class ServiceManager
    {
        public int GetServiceId(string serviceName)
        {
            using (var context = new PaymentWFEntities())
            {
                return (context.Services.Where(q => q.Name == serviceName).Select(q => q.Id)).FirstOrDefault();
            }
        }

        public string GetServiceName(int serviceId)
        {
            using (var context = new PaymentWFEntities())
            {
                return context.Services.FirstOrDefault(x => x.Id == serviceId).Name;
            }
        }

        public IList<Services> GetServices(PaymentWFEntities context)
        {
            return context.Services.ToList();
        }
    }
}
