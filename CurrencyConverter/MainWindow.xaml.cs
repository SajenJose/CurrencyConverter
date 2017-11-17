using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //statusBarText.Text = "Click on Convert to begin...";
        }




        private void buttonConvert_Click(object sender, RoutedEventArgs e)
        {
            var inputAmount = txtBoxAmount.Text;
            var amount = Convert.ToDecimal(inputAmount);
            var srcInputCurrency = cmbBoxSrcCurrency.Text;
            string srcCurrency = null;
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

            CallConversionEngine(amount, srcCurrency);

        }

        private void CallConversionEngine(decimal amount, string srcCurrency)
        {
            lblINRValue.Content = ConversionEngine(amount, srcCurrency, "INR");
            lblUSDValue.Content = ConversionEngine(amount, srcCurrency, "USD");
            lblGBPValue.Content = ConversionEngine(amount, srcCurrency, "GBP");
            lblEURValue.Content = ConversionEngine(amount, srcCurrency, "EUR");
            lblSGDValue.Content = ConversionEngine(amount, srcCurrency, "SGD");
            lblAUDValue.Content = ConversionEngine(amount, srcCurrency, "AUD");
            lblNZDValue.Content = ConversionEngine(amount, srcCurrency, "NZD");
            lblJPYValue.Content = ConversionEngine(amount, srcCurrency, "JPY");
        }

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
