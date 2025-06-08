using FinTracker.Data;
using FinTracker.ViewModels;
using FinTracker.ViewModels.Accounts;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для AddOperationWindow.xaml
    /// </summary>
    public partial class AddOperationWindow : Window
    {
        public AddOperationWindow()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>();
            services.AddTransient<AddOperationViewModel>();
            var provider = services.BuildServiceProvider();
            DataContext = provider.GetRequiredService<AddOperationViewModel>();

        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AddOperationViewModel viewModel = (AddOperationViewModel)DataContext;
            viewModel.ExecuteOperation();
            this.Close();
        }
    }
}
