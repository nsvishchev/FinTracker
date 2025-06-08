// ViewModels/AccountsViewModel.cs
using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FinTracker.ViewModels
{
    public class DeleteAccountViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private ObservableCollection<string> _accountNames;
        private string _selectedAccountName;

        public ObservableCollection<string> AccountNames
        {
            get => _accountNames;
            set
            {
                _accountNames = value;
                OnPropertyChanged(nameof(AccountNames));
            }
        }

        public string SelectedAccountName
        {
            get => _selectedAccountName;
            set
            {
                _selectedAccountName = value;
                OnPropertyChanged(nameof(SelectedAccountName));
                // Дополнительные действия при выборе
            }
        }

        public DeleteAccountViewModel(AppDbContext context)
        {
            _context = context;
            LoadAccountNamesAsync().ConfigureAwait(false);
        }

        public async Task LoadAccountNamesAsync()
        {
            try
            {
                var names = await _context.Accounts
                    .OrderBy(a => a.Name)
                    .Select(a => a.Name)
                    .ToListAsync();

                AccountNames = new ObservableCollection<string>(names);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки счетов: {ex.Message}");
            }
        }
        public async Task DeleteAccount()
        {
            try
            {
                var names = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == SelectedAccountName);
                _context.Accounts.Remove(names);
                await _context.SaveChangesAsync();
                MessageBox.Show("Счет успешно удален!");
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Ошибка: {ex}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}