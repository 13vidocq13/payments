using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DataAccess;
using Payments.Help;
using System.Linq;
using System.Globalization;
using Payments.Properties;

namespace Payments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Debugger.Break();

            if (labelDate == null)
                labelDate = new Label();

            labelDate.Text = DateTime.Now.ToString();
            SetLocale();
            InitializeComponent();
            SetSelectedLocaleInCombobx();
            BindMonth();
            BindYears();
        }

        private void buttonStat_Click(object sender, EventArgs e)
        {
            var formStat = new Form();
            formStat.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var s = Resources.ErrorText;
        }

        private void Show_Click(object sender, EventArgs e)
        {
            if (Month_CB.SelectedIndex == 0)
            {
                MessageBox.Show(Resources.MonthNotSelected, Resources.ErrorText, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Year_CB.SelectedIndex == 0)
            {
                MessageBox.Show(Resources.YearNotSelected,
                    Resources.ErrorText,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            labelSum.Text = null;
            labelSelectedYear.Text = Year_CB.SelectedItem.ToString();
            labelSelectedMonth.Text = ((ComboBoxItem)(Month_CB.SelectedItem)).Value;

            BindSelectedData(new PayManager().GetData((int)Year_CB.SelectedItem,
                ((ComboBoxItem)(Month_CB.SelectedItem)).Key));

            labelSum.Text = ResultCalculate() + " грн";
        }

        private void Pay_Click(object sender, EventArgs e)
        {
            if (Month_CB.SelectedIndex == 0)
            {
                MessageBox.Show(Resources.MonthNotSelected,
                    Resources.ErrorText,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (Year_CB.SelectedIndex == 0)
            {
                MessageBox.Show(Resources.YearNotSelected,
                    Resources.ErrorText,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Difference();
            SumCalculate();
            labelSum.Text = ResultCalculate() + " грн";

            if (MessageBox.Show(Resources.AreYouSure,
                Resources.Confirm,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var data = BindPay();
            new PayManager().SaveData(data, (int)Year_CB.SelectedItem,
                ((ComboBoxItem)(Month_CB.SelectedItem)).Key);

            MessageBox.Show(Resources.PaymentSuccess,
                Resources.Message,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Properties.Settings.Default.CurrentLocale = "uk-UA";
                    break;
                case 1:
                    Properties.Settings.Default.CurrentLocale = "ru-RU";
                    break;
                case 2:
                    Properties.Settings.Default.CurrentLocale = "en-US";
                    break;
                case 3:
                    Properties.Settings.Default.CurrentLocale = "fr-FR";
                    break;
                case 4:
                    Properties.Settings.Default.CurrentLocale = "es-ES";
                    break;
                case 5:
                    Properties.Settings.Default.CurrentLocale = "de-DE";
                    break;
                case 6:
                    Properties.Settings.Default.CurrentLocale = "ja-JP";
                    break;
                case 7:
                    Properties.Settings.Default.CurrentLocale = "ko-KR";
                    break;
                case 8:
                    Properties.Settings.Default.CurrentLocale = "he-IL";
                    break;
            }

            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new TariffsForm().ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Restart();
        }

        //если нет данных, заполняем с тарифов
        //а так же заполняем показания счетчика с прошлого месяца
        void BindDataNewMonth()
        {
            var currentMonth = ((ComboBoxItem)(Month_CB.SelectedItem)).Key;
            int previousMonth;
            var year = (int)Year_CB.SelectedItem;

            if (currentMonth - 1 == 0)
            {
                previousMonth = 12;
                year -= 1;
            }
            else
                previousMonth = currentMonth - 1;

            var previousData = new PayManager().GetData(year, previousMonth);

            var entity = previousData.FirstOrDefault(x => x.IdService == 1);
            if (entity != null) CounterEEn1.Text = entity.CounterSecond.ToString();

            entity = previousData.FirstOrDefault(x => x.IdService == 4);
            if (entity != null) CounterGov1.Text = entity.CounterSecond.ToString();

            entity = previousData.FirstOrDefault(x => x.IdService == 5);
            if (entity != null) CounterHov1.Text = entity.CounterSecond.ToString();

            entity = previousData.FirstOrDefault(x => x.IdService == 5);
            if (entity != null) CounterKan1.Text = entity.CounterSecond.ToString();

            KvpSum.Text = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                                                   .GetServiceId("Rent")).Price.ToString();

            GavSum.Text = GavSum.Text = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                                                                 .GetServiceId("GasOfVarilny"))
                                                                                 .Price.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SetSelectedLocaleInCombobx()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.CurrentLocale))
                comboBox1.SelectedIndex = 0;
            else
            {
                switch ((Properties.Settings.Default.CurrentLocale))
                {
                    case "uk-UA":
                        comboBox1.SelectedIndex = 0;
                        break;
                    case "ru-RU":
                        comboBox1.SelectedIndex = 1;
                        break;
                    case "en-US":
                        comboBox1.SelectedIndex = 2;
                        break;
                    case "fr-FR":
                        comboBox1.SelectedIndex = 3;
                        break;
                    case "es-ES":
                        comboBox1.SelectedIndex = 4;
                        break;
                    case "de-DE":
                        comboBox1.SelectedIndex = 5;
                        break;
                    case "ja-JP":
                        comboBox1.SelectedIndex = 6;
                        break;
                    case "ko-KR":
                        comboBox1.SelectedIndex = 7;
                        break;
                    case "he-IL":
                        comboBox1.SelectedIndex = 8;
                        break;
                }
            }
        }

        private static void SetLocale()
        {
            switch ((Properties.Settings.Default.CurrentLocale))
            {
                case "uk-UA":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("uk-UA");
                    break;
                case "ru-RU":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    break;
                case "en-US":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    break;
                case "fr-FR":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
                    break;
                case "es-ES":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                    break;
                case "de-DE":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                    break;
                case "ja-JP":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");
                    break;
                case "ko-KR":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ko-KR");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ko-KR");
                    break;
                case "he-IL":
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("he-IL");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("he-IL");
                    break;
            }
        }

        //заполнение данных для отображения выборки
        void BindSelectedData(IEnumerable<Pays> data)
        {
            ClearData();

            if (data.Count() == 0)
            {
                BindDataNewMonth();
                return;
            }

            //электроэнергия
            var item = data.FirstOrDefault(x => x.IdService == 1);
            if (item != null)
            {
                CounterEEn1.Text = item.CounterFirst.ToString();
                CounterEEn2.Text = item.CounterSecond.ToString();
                DifferenceEEn.Text = item.Difference.ToString();
                SumEEn.Text = item.Sum.ToString();
            }

            //квартплата
            item = data.FirstOrDefault(x => x.IdService == 2);
            KvpSum.Text = item == null ? null : item.Sum.ToString();

            //отопление
            item = data.FirstOrDefault(x => x.IdService == 3);
            OpaSum.Text = item == null ? null : item.Sum.ToString();

            //гарячая вода
            item = data.FirstOrDefault(x => x.IdService == 4);
            if (item != null)
            {
                CounterGov1.Text = item.CounterFirst.ToString();
                CounterGov2.Text = item.CounterSecond.ToString();
                DifferenceGov.Text = item.Difference.ToString();
                GovSum.Text = item.Sum.ToString();
            }

            //холодная вода
            item = data.FirstOrDefault(x => x.IdService == 5);
            if (item != null)
            {
                CounterHov1.Text = item.CounterFirst.ToString();
                CounterHov2.Text = item.CounterSecond.ToString();
                DifferenceHov.Text = item.Difference.ToString();
                HovSum.Text = item.Sum.ToString();
            }

            //канализация
            item = data.FirstOrDefault(x => x.IdService == 6);
            if (item != null)
            {
                CounterKan1.Text = item.CounterFirst.ToString();
                CounterKan2.Text = item.CounterSecond.ToString();
                DifferenceKan.Text = item.Difference.ToString();
                KanSum.Text = item.Sum.ToString();
            }

            //газ варильный
            item = data.FirstOrDefault(x => x.IdService == 7);
            if (item != null) GavSum.Text = item.Sum.ToString();

            //газ отопительный
            item = data.FirstOrDefault(x => x.IdService == 8);
            GaoSum.Text = item == null ? null : item.Sum.ToString();
        }

        //заполнение данных для сохранения
        IEnumerable<Pays> BindPay()
        {
            var items = new List<Pays>();

            double tempTryParse;
            int tempIntPare;

            //электроэнергия
            var item = new Pays
                           {
                               CounterFirst =
                                   double.TryParse(CounterEEn1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                               CounterSecond = double.TryParse(CounterEEn2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                               IdService = 1,
                               Difference = int.TryParse(DifferenceEEn.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                               Sum = double.TryParse(SumEEn.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                               IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("ElectricPower")).Id
                           };

            items.Add(item);

            //квартплата
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterKvp1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterKvp2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 2,
                Difference = int.TryParse(DifferenceKvp.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(KvpSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("Rent")).Id
            };

            items.Add(item);

            //отопление
            item = new Pays
            {
                CounterFirst = null,
                CounterSecond = null,
                IdService = 3,
                Difference = null,
                Sum = double.TryParse(OpaSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = 3
            };

            items.Add(item);

            //гарячая вода
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterGov1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterGov2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 4,
                Difference = int.TryParse(DifferenceGov.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(GovSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("HotWater")).Id
            };

            items.Add(item);

            //холодная вода
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterHov1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterHov2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 5,
                Difference = int.TryParse(DifferenceHov.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(HovSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("ColdWater")).Id
            };

            items.Add(item);

            //канализация
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterKan1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterKan2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 6,
                Difference = int.TryParse(DifferenceKan.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(KanSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("WateDrain")).Id
            };

            items.Add(item);

            //газ варильный
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterGav1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterGav2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 7,
                Difference = int.TryParse(DifferenceGav.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(GavSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null,
                IdTariff = new TariffsManager().GetCurrentTariff(new ServiceManager()
                                            .GetServiceId("GasOfVarilny")).Id
            };

            items.Add(item);

            //газ отопительный
            item = new Pays
            {
                CounterFirst =
                    double.TryParse(CounterGao1.Text, out tempTryParse) ? (double?)tempTryParse : null,
                CounterSecond = double.TryParse(CounterGao2.Text, out tempTryParse) ? (double?)tempTryParse : null,
                IdService = 8,
                Difference = int.TryParse(DifferenceGao.Text, out tempIntPare) ? (int?)(tempIntPare) : null,
                Sum = double.TryParse(GaoSum.Text, out tempTryParse) ? (double?)(tempTryParse) : null
            };

            items.Add(item);

            return items;
        }

        void ClearData()
        {
            CounterEEn1.Text = null;
            CounterEEn2.Text = null;
            DifferenceEEn.Text = null;
            SumEEn.Text = null;

            KvpSum.Text = null;
            OpaSum.Text = null;

            CounterGov1.Text = null;
            CounterGov2.Text = null;
            DifferenceGov.Text = null;
            GovSum.Text = null;

            CounterHov1.Text = null;
            CounterHov2.Text = null;
            DifferenceHov.Text = null;
            HovSum.Text = null;

            CounterKan1.Text = null;
            CounterKan2.Text = null;
            DifferenceKan.Text = null;
            KanSum.Text = null;

            GaoSum.Text = null;
        }

        void BindMonth()
        {
            //не пашет блл
            var months = new MonthManager().GetMonths(Thread.CurrentThread.CurrentCulture.ToString());

            Month_CB.Items.Add(new ComboBoxItem { Key = -1, Value = Resources.NothingSelected });
            foreach (var item in months.Select(month => new ComboBoxItem { Key = month.Nubmer, Value = month.Title }))
            {
                Month_CB.Items.Add(item);
            }

            Month_CB.DisplayMember = "Value";
            Month_CB.SelectedIndex = 0;
        }

        void BindYears()
        {
            Year_CB.Items.Add(Resources.NothingSelected);

            for (var i = 2000; i < DateTime.Now.Year + 1; i++)
            {
                Year_CB.Items.Add(i);
            }
            Year_CB.SelectedIndex = 0;
        }

        void Difference()
        {
            int counter1, counter2;

            if (int.TryParse(CounterEEn2.Text, out counter2) && int.TryParse(CounterEEn1.Text, out counter1))
                DifferenceEEn.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterKvp2.Text, out counter2) && int.TryParse(CounterKvp1.Text, out counter1))
                DifferenceKvp.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterOpa2.Text, out counter2) && int.TryParse(CounterOpa1.Text, out counter1))
                DifferenceOpa.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterGov2.Text, out counter2) && int.TryParse(CounterGov1.Text, out counter1))
                DifferenceGov.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterHov2.Text, out counter2) && int.TryParse(CounterHov1.Text, out counter1))
                DifferenceHov.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterKan2.Text, out counter2) && int.TryParse(CounterKan1.Text, out counter1))
                DifferenceKan.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterGav2.Text, out counter2) && int.TryParse(CounterGav1.Text, out counter1))
                DifferenceGav.Text = (counter2 - counter1).ToString();

            if (int.TryParse(CounterGao2.Text, out counter2) && int.TryParse(CounterGao1.Text, out counter1))
                DifferenceGao.Text = (counter2 - counter1).ToString();
        }

        void SumCalculate()
        {
            var sum = EenCalculate();

            //из-за возможности внести вручную сумму
            if (sum != null)
                SumEEn.Text = Math.Round((double)sum, 2).ToString();

            sum = GovCalculate();
            GovSum.Text = sum != null ? Math.Round((double)sum, 2).ToString() : "";

            sum = HovCalculate();
            HovSum.Text = sum != null ? Math.Round((double)sum, 2).ToString() : "";

            sum = KanCalculate();
            KanSum.Text = sum != null ? Math.Round((double)sum, 2).ToString() : "";

            //на случай, если поменялись тарифы
            KvpSum.Text =
                new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("Rent")).Price.ToString();
            GavSum.Text = new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("GasOfVarilny")).Price.ToString();
        }

        double? GovCalculate()
        {
            int dif;
            var tariff = new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("HotWater"));

            if (!int.TryParse(DifferenceGov.Text, out dif))
                return null;

            return dif * tariff.Price;
        }

        double? HovCalculate()
        {
            int difG, difH;
            var tariff = new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("ColdWater"));

            if (!int.TryParse(DifferenceGov.Text, out difG) ||
                !int.TryParse(DifferenceHov.Text, out difH))
                return null;

            return (difG + difH) * tariff.Price;
        }

        double? KanCalculate()
        {
            int difG, difK;
            var tariff = new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("WateDrain"));

            if (!int.TryParse(DifferenceGov.Text, out difG) ||
                !int.TryParse(DifferenceKan.Text, out difK))
                return null;

            return (difG + difK) * tariff.Price;
        }

        double? EenCalculate()
        {
            var tariffs = new TariffsManager().GetCurrentTariff(new ServiceManager().GetServiceId("ElectricPower"));
            int dif;

            if (!int.TryParse(DifferenceEEn.Text, out dif))
                return null;

            if (dif <= 100)
            {
                return dif * tariffs.Price;
            }

            return (dif - 100) * tariffs.Overexpenditure + 100 * tariffs.Price;
        }

        double ResultCalculate()
        {
            double een;
            double.TryParse(SumEEn.Text, out een);

            double kvp;
            double.TryParse(KvpSum.Text, out kvp);

            double opa;
            double.TryParse(OpaSum.Text, out opa);

            double gov;
            double.TryParse(GovSum.Text, out gov);

            double hov;
            double.TryParse(HovSum.Text, out hov);

            double kan;
            double.TryParse(KanSum.Text, out kan);

            double gav;
            double.TryParse(GavSum.Text, out gav);

            double gao;
            double.TryParse(GaoSum.Text, out gao);

            return een + kvp + opa + gov + hov + kan + gav + gao;
        }


    }
}
