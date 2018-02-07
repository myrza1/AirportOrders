using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirportOrders
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string _Token = ConfigurationManager.AppSettings["Token"].ToString();
        string timerKezegi = ConfigurationManager.AppSettings["timerKezegi"].ToString();
        string updateTime = ConfigurationManager.AppSettings["UpdateTime"].ToString();
        System.Timers.Timer j = new System.Timers.Timer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            AirportOrders.eAirlinesDataSet eAirlinesDataSet = ((AirportOrders.eAirlinesDataSet)(this.FindResource("eAirlinesDataSet")));
            // Загрузить данные в таблицу vArrivalFlight. Можно изменить этот код как требуется.
            AirportOrders.eAirlinesDataSetTableAdapters.vArrivalFlightTableAdapter eAirlinesDataSetvArrivalFlightTableAdapter = new AirportOrders.eAirlinesDataSetTableAdapters.vArrivalFlightTableAdapter();
            eAirlinesDataSetvArrivalFlightTableAdapter.Fill(eAirlinesDataSet.vArrivalFlight);
            System.Windows.Data.CollectionViewSource vArrivalFlightViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("vArrivalFlightViewSource")));
            vArrivalFlightViewSource.View.MoveCurrentToFirst();

        }


        private void vArrivalFlightDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadDate.Text = "2007-10-5";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            LoadDate.Text = "01-01-2018";
        }

        private void btnONOFF_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                DateTime bugin = DateTime.Now;
                String yourText = bugin.ToString() + " " + "Timer Started" + Environment.NewLine;
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Session.log", yourText);

                //Создаем таймер и выставляем его параметры




                
                j.Enabled = true;

                //Интервал 10000мс - 10с.
                j.Interval = Convert.ToInt16( updateTime);
                j.Elapsed += new System.Timers.ElapsedEventHandler(TimerGo);
                j.AutoReset = true;
                j.Start();

            }
            catch (Exception ex)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Session.log", e.ToString());
            }
        }
        private void TimerGo(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (timerKezegi == "ScanWeb")
            {
               // this.LoadDate.Text = "2007-10-6";
                scanWeb(sender);
            }
            else if (timerKezegi == "LDM")
            {
                scanWeb(sender);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Session.log", "ldm--");
            }
           
        }

        private void scanWeb(object sender)
        {
            Dispatcher.Invoke(new Action(() => LoadDate.Text = "2007-10-6"));


        }
    }
}
