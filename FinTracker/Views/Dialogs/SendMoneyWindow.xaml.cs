﻿using FinTracker.Data;
using FinTracker.ViewModels;
using FinTracker.ViewModels.Transactions;
using Microsoft.Extensions.DependencyInjection;
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
    /// Логика взаимодействия для SendMoneyWindow.xaml
    /// </summary>
    public partial class SendMoneyWindow : Window
    {
        public SendMoneyWindow()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();
            services.AddTransient<SendMoneyViewModel>();
            var provider = services.BuildServiceProvider();
            DataContext = provider.GetRequiredService<SendMoneyViewModel>();
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            SendMoneyViewModel sendMoneyViewModel = (SendMoneyViewModel)DataContext;
            sendMoneyViewModel.ExecuteTransferAsync();
            this.Close();
        }
    }
}
