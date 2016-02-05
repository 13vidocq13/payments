using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace DataAccess
{
    public class PayManage
    {
        public IEnumerable<Pays> GetData(int year, int monthId)
        {
            using (var context = new PaymentWFEntities())
            {
                return context.Pays.Where(x => x.Year == year).Where(x => x.IdMonth == monthId).ToList();
            }
        }

        public void SaveData(IEnumerable<Pays> data, int year, int monthId)
        {
            using (var context = new PaymentWFEntities())
            {
                using (var transaction = new TransactionScope())
                {
                    if (IsHaveData(year, monthId, context))
                    {
                        foreach (var item in data)
                        {
                            var currentItem = GetPaymentItem(year, monthId, item.IdService, context);

                            currentItem.CounterFirst = item.CounterFirst;
                            currentItem.CounterSecond = item.CounterSecond;
                            currentItem.Difference = item.Difference;
                            currentItem.Sum = item.Sum;
                            currentItem.IdTariff = item.IdTariff;

                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        foreach (var newItem in data.Select(item => new Pays
                                                                        {
                                                                            Year = year,
                                                                            IdMonth = monthId,
                                                                            IdService = item.IdService,
                                                                            CounterFirst = item.CounterFirst,
                                                                            CounterSecond = item.CounterSecond,
                                                                            Difference = item.Difference,
                                                                            Sum = item.Sum,
                                                                            IdTariff = item.IdTariff
                                                                        }))
                        {
                            context.Pays.Add(newItem);
                            context.SaveChanges();
                        }
                    }

                    transaction.Complete();
                }
            }
        }

        static Pays GetPaymentItem(int year, int monthId, int? idService, PaymentWFEntities context)
        {
            return
                (context.Pays.Where(x => x.Year == year).Where(x => x.IdMonth == monthId).Where(
                    x => x.IdService == idService)).FirstOrDefault();
        }

        static bool IsHaveData(int year, int monthId, PaymentWFEntities context)
        {
            return (context.Pays.Where(q => q.IdMonth == monthId).Where(q => q.Year == year)).Any();
        }
    }
}
