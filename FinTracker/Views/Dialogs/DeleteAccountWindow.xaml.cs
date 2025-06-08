using FinTracker.Data;
using FinTracker.ViewModels;
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
    /// Логика взаимодействия для DeleteAccountWindow.xaml
    /// </summary>
    public partial class DeleteAccountWindow : Window
    {
        public DeleteAccountWindow()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();
            services.AddTransient<DeleteAccountViewModel>();
            var provider = services.BuildServiceProvider();
            DataContext = provider.GetRequiredService<DeleteAccountViewModel>();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteAccountViewModel deleteAccointViewModel = (DeleteAccountViewModel)DataContext;
            deleteAccointViewModel.DeleteAccount();
            this.Close();
        }

    }
}
