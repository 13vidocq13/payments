using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataAccess;
using Payments.Properties;

namespace Payments
{
    public partial class TariffsForm : Form
    {
        private IList<Tariffs> _tariffses;

        public TariffsForm()
        {
            InitializeComponent();
            StartupBinding();    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var changeList = PreSaveCheck();

            if (changeList.Count == 0)
            {
                MessageBox.Show(Resources.ChangesNotFound, Resources.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            foreach (var item in changeList)
            {
                new TariffsManager().SaveTariff(item);
            }

            MessageBox.Show(Resources.ChangesSaved, Resources.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        void StartupBinding()
        {
            _tariffses = new TariffsManager().GetCurrentTariffs();

            foreach (var item in _tariffses)
            {
                switch (new ServiceManager().GetServiceName(item.ServiceId))
                {
                    case "ElectricPower":
                        tbElectricPower.Text = item.Price.ToString();
                        tbElectricPower2.Text = item.Overexpenditure.ToString();
                        break;
                    case "Rent":
                        tbRent.Text = item.Price.ToString();
                        break;
                    case "HotWater":
                        tbHotWater.Text = item.Price.ToString();
                        break;
                    case "ColdWater":
                        tbColdWater.Text = item.Price.ToString();
                        break;
                    case "WateDrain":
                        tbWateDrain.Text = item.Price.ToString();
                        break;
                    case "GasOfVarilny":
                        tbGasOfVarilny.Text = item.Price.ToString();
                        break;
                }
            }
        }

        //проверка изменений, чтоб не перезаписывать все тарифы
        IList<Tariffs> PreSaveCheck()
        {
            IList<Tariffs> res = new List<Tariffs>();

            foreach (var item in _tariffses)
            {
                double newPrice;
                switch (new ServiceManager().GetServiceName(item.ServiceId))
                {
                    case "ElectricPower":
                        double newOverexpenditure;
                        if(!double.TryParse(tbElectricPower.Text, out newPrice) ||
                            !double.TryParse(tbElectricPower2.Text, out newOverexpenditure))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbElectricPower.Text != item.Price.ToString() ||
                            tbElectricPower2.Text != item.Overexpenditure.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            Overexpenditure = newOverexpenditure,
                                            ServiceId = new ServiceManager().GetServiceId("ElectricPower")
                                        });
                            break;
                    case "Rent":
                        if(!double.TryParse(tbRent.Text, out newPrice))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbRent.Text != item.Price.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            ServiceId = new ServiceManager().GetServiceId("Rent")
                                        });
                        break;
                    case "HotWater":
                        if(!double.TryParse(tbHotWater.Text, out newPrice))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbHotWater.Text != item.Price.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            ServiceId = new ServiceManager().GetServiceId("HotWater")
                                        });
                        break;
                    case "ColdWater":
                         if(!double.TryParse(tbColdWater.Text, out newPrice))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbColdWater.Text != item.Price.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            ServiceId = new ServiceManager().GetServiceId("ColdWater")
                                        });
                        break;
                    case "WateDrain":
                        if(!double.TryParse(tbWateDrain.Text, out newPrice))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbWateDrain.Text != item.Price.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            ServiceId = new ServiceManager().GetServiceId("WateDrain")
                                        });
                        break;
                    case "GasOfVarilny":
                        if(!double.TryParse(tbGasOfVarilny.Text, out newPrice))
                            throw new InvalidOperationException("Недопустимий формат");
                        if (tbGasOfVarilny.Text != item.Price.ToString())
                            res.Add(new Tariffs
                                        {
                                            DateSet = DateTime.Now,
                                            Price = newPrice,
                                            ServiceId = new ServiceManager().GetServiceId("GasOfVarilny")
                                        });
                        break;
                }
            }

            return res;
        }
    }
}
