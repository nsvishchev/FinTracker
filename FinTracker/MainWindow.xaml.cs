using FinTracker.ViewModels;
using FinTracker.ViewModels.Accounts;
using FinTracker.Views;
using FinTracker.Views.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Net.Security;
using System.Text;
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

namespace FinTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            Loaded += async (sender, args) => await ((MainViewModel)DataContext).AccountVM.LoadAccountAsync();
        }

        private void AddOpertationButton_Click(object sender, RoutedEventArgs e)
        {
            AddOperationWindow addOperationWindow = new AddOperationWindow();
            addOperationWindow.Owner = this;
            addOperationWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addOperationWindow.Show();
        }
        private void ViewAllAccountList_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddAccount_Button_Click(object sender, RoutedEventArgs e)
        {
            var addAcoountWindow = new AddAccountWindow();
            addAcoountWindow.Owner = this;
            addAcoountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            addAcoountWindow.Show();

        }
        private async void AccountWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            await vm.AccountVM.LoadAccountAsync();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void CreateTargetButton_Click(object sender, RoutedEventArgs e)
        {
            TargetWindow_Loaded(sender, e);
            var AddTargetWindow = new AddTargetWindow();
            AddTargetWindow.Owner = this;
            AddTargetWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //TargetWindow_Loaded(sender, e);
            var vm = (MainViewModel)this.DataContext;
            vm.TargetVM.LoadTargetAsync();

            AddTargetWindow.ShowDialog();
            NoTargetPanel.Visibility = Visibility.Collapsed;
            YesTargetPanel.Visibility = Visibility.Visible;
        }
        private async void TargetWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            await vm.TargetVM.LoadTargetAsync();
        }
        private void DeleteTargetButton_Click(object sender, RoutedEventArgs e)
        {
            var MainViewModel = new MainViewModel();
            MainViewModel.TargetVM.DeleteTargetAsync();


            NoTargetPanel.Visibility = Visibility.Visible;
            YesTargetPanel.Visibility = Visibility.Collapsed;
        }

        private void DeleteAccount_Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteAccountWindow deleteAccountWindow = new DeleteAccountWindow();
            deleteAccountWindow.Owner = this;
            deleteAccountWindow.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            deleteAccountWindow.Show();
        }

        private void OpertationsHistory_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewTransactions = new ViewTransactions();
            viewTransactions.Owner = this;
            viewTransactions.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            viewTransactions.Show();
        }

        private void MoneyTransfer_Button_Click(object sender, RoutedEventArgs e)
        {
            var sendMoneyWindow = new SendMoneyWindow();
            sendMoneyWindow.Owner = this;
            sendMoneyWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sendMoneyWindow.Show();
        }
    }
}