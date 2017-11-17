using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public string inputAmount;
        public decimal amount;
        public string srcCurrency;
        public string srcInputCurrency;

        public MainWindow()
        {
            InitializeComponent();
            statusBarText.Text = "Status: ";
            backgroundWorker.WorkerReportsProgress = true; //progress bar
            backgroundWorker.ProgressChanged += ProgressChanged; //progress bar
            backgroundWorker.DoWork += DoWork; //progress bar
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted; //progress bar

        }


        //Progress bar do work method
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            string[] targetCurrency = { "INR", "USD", "GBP", "EUR", "SGD", "AUD", "NZD", "JPY" };
            Label[] labelarray = { lblINRValue, lblUSDValue, lblGBPValue, lblEURValue, lblSGDValue, lblAUDValue, lblNZDValue, lblJPYValue };
            for (int i = 0; i < 8; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    labelarray[i].Content = Math.Round(ConversionEngine(amount, srcCurrency, targetCurrency[i]), 2);
                });
                backgroundWorker.ReportProgress(i);
            }




        }

        //Progress bar code
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        //Progress bar code
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Clean up code
        }


        //Main Engine Initialized
        private void buttonConvert_Click(object sender, RoutedEventArgs e)
        {
            pbStatus.Value = 0;

            
            inputAmount = txtBoxAmount.Text;
            amount = Convert.ToDecimal(inputAmount);
            srcInputCurrency = cmbBoxSrcCurrency.Text;
            srcCurrency = null;
            switch (srcInputCurrency)
            {
                case "Indian Rupees (INR)":
                    srcCurrency = "INR";
                    break;

                case "US Dollar (USD)":
                    srcCurrency = "USD";
                    break;

                case "British Pound (GBP)":
                    srcCurrency = "GBP";
                    break;

                case "EU Euro (EUR)":
                    srcCurrency = "EUR";
                    break;

                case "Singapore Dollar (SGD)":
                    srcCurrency = "SGD";
                    break;

                case "Australian Dollar (AUD)":
                    srcCurrency = "AUD";
                    break;

                case "NZ Dollar (NZD)":
                    srcCurrency = "NZD";
                    break;

                case "Japanese Yen (JPY)":
                    srcCurrency = "JPY";
                    break;
            }

            pbStatus.Minimum = 0;
            pbStatus.Maximum = 7;
            backgroundWorker.RunWorkerAsync();

        }

        // Conversion Engine
        public static decimal ConversionEngine(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                if (fromCurrency == toCurrency)
                {
                    return amount;
                }
                else
                {
                    WebClient web = new WebClient();

                    string url = string.Format("https://finance.google.com/finance/converter?a={2}&from={0}&to={1}", fromCurrency.ToUpper(), toCurrency.ToUpper(), amount);

                    string respone = web.DownloadString(url);

                    Regex regex = new Regex(@"(\d+(\.\d*)?)\s" + toCurrency);

                    var r = regex.Match(respone);

                    return Convert.ToDecimal(r.Groups[1].Value);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        // Shutdown application
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            About abtWindow = new About();
            abtWindow.ShowInTaskbar = false;
            abtWindow.Owner = Application.Current.MainWindow;
            abtWindow.Show();
        }

        // Load Combo Box
        private void Window_Loaded(object sender, EventArgs e)
        {
            cmbBoxSrcCurrency.Items.Add("Indian Rupees (INR)");
            cmbBoxSrcCurrency.Items.Add("US Dollar (USD)");
            cmbBoxSrcCurrency.Items.Add("British Pound (GBP)");
            cmbBoxSrcCurrency.Items.Add("EU Euro (EUR)");
            cmbBoxSrcCurrency.Items.Add("Singapore Dollar (SGD)");
            cmbBoxSrcCurrency.Items.Add("Australian Dollar (AUD)");
            cmbBoxSrcCurrency.Items.Add("NZ Dollar (NZD)");
            cmbBoxSrcCurrency.Items.Add("Japanese Yen (JPY)");
            cmbBoxSrcCurrency.SelectedValue = "Indian Rupees (INR)";
        }
    }
}
