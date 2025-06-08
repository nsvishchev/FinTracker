using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FinTracker.ViewModels
{
    public class AddOperationViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;

        // Свойства для ComboBox с типами операций
        public ObservableCollection<string> OperationTypes { get; } = new ObservableCollection<string>
        {
            "Пополнить",
            "Списать"
        };

        private string _selectedOperationType;
        public string SelectedOperationType
        {
            get => _selectedOperationType;
            set
            {
                _selectedOperationType = value;
                OnPropertyChanged(nameof(SelectedOperationType));
                // Можно добавить дополнительную логику при изменении выбора
            }
        }

        // Свойства для ComboBox со счетами
        public ObservableCollection<string> AccountNames { get; set; }
        private string _selectedAccountName;
        public string SelectedAccountName
        {
            get => _selectedAccountName;
            set
            {
                _selectedAccountName = value;
                OnPropertyChanged(nameof(SelectedAccountName));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public AddOperationViewModel(AppDbContext context)
        {
            _context = context;
            SelectedOperationType = OperationTypes.First(); // Устанавливаем первый элемент по умолчанию
            LoadAccountsAsync().ConfigureAwait(false);
        }

        public async Task LoadAccountsAsync()
        {
            try
            {
                var names = await _context.Accounts
                    .OrderBy(a => a.Name)
                    .Select(a => a.Name)
                    .ToListAsync();

                AccountNames = new ObservableCollection<string>(names);
                OnPropertyChanged(nameof(AccountNames));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки счетов: {ex.Message}");
            }
        }

        public async Task ExecuteOperation()
        {
            if (string.IsNullOrEmpty(SelectedAccountName) || Amount <= 0)
            {
                MessageBox.Show("Выберите счет и укажите сумму больше нуля");
                return;
            }

            try
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Name == SelectedAccountName);

                if (account == null)
                {
                    MessageBox.Show("Счет не найден");
                    return;
                }

                if (SelectedOperationType == "Пополнить")
                {
                    account.Balance += Amount;

                    var transaction = new Transaction
                        { 
                        Operation = "Пополнение",
                        AccountId = account.Id,
                        Amount = Amount 
                        };
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                }
                else
                {
                    if (account.Balance < Amount)
                    {
                        MessageBox.Show("Недостаточно средств на счете");
                        return;
                    }
                    account.Balance -= Amount;
                    var transaction = new Transaction
                    {
                        Operation = "Списание",
                        AccountId = account.Id,
                        Amount = Amount
                    };
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                }

                await _context.SaveChangesAsync();
                MessageBox.Show("Операция выполнена успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}