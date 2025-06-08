using FinTracker.ViewModels.Transactions;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FinTracker.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ViewTransactions.xaml
    /// </summary>
    public partial class ViewTransactions : Window
    {
        public ViewTransactions()
        {
            InitializeComponent();
            DataContext = new TransactrionViewModel();
            Loaded += async (sender, args) => await ((TransactrionViewModel)DataContext).LoadTransactionsAsync();

        }
        public void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
