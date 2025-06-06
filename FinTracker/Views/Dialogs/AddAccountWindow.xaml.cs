using FinTracker.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FinTracker.Views
{
    /// <summary>
    /// Логика взаимодействия для AddAccount.xaml
    /// </summary>
    public partial class AddAccountWindow : Window
    {
        public AddAccountWindow()
        {
            InitializeComponent();
            this.DataContext = new AccountViewModel();
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (AccountViewModel)this.DataContext;

            model.AddAccountAsync();

            this.Close();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
