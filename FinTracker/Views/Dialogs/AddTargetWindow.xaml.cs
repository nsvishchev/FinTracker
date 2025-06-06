using System.Windows;
using System.Windows.Controls;
using FinTracker.ViewModels;

namespace FinTracker
{
    /// <summary>
    /// Логика взаимодействия для AddTargetWindow.xaml
    /// </summary>
    public partial class AddTargetWindow : Window
    {
        public AddTargetWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.DataContext = new TargetViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (TargetViewModel)this.DataContext;

            _ = model.AddTargetAsync();

            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
