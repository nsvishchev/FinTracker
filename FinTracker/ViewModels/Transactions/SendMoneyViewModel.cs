using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;

namespace FinTracker.ViewModels.Transactions
{
    class SendMoneyViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private string _fromSelectedAccountName;
        private string _toSelectedAccountName;
        private decimal _amount;

        public ObservableCollection<string> FromAccountNames { get; private set; }
        public ObservableCollection<string> ToAccountNames { get; private set; }

        public string FromSelectedAccountName
        {
            get => _fromSelectedAccountName;
            set
            {
                if (_fromSelectedAccountName != value)
                {
                    _fromSelectedAccountName = value;
                    OnPropertyChanged(nameof(FromSelectedAccountName));
                    // При изменении счета-отправителя обновляем список получателей
                    LoadToAccountsAsync().ConfigureAwait(false);
                }
            }
        }

        public string ToSelectedAccountName
        {
            get => _toSelectedAccountName;
            set
            {
                if (_toSelectedAccountName != value)
                {
                    _toSelectedAccountName = value;
                    OnPropertyChanged(nameof(ToSelectedAccountName));
                }
            }
        }
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        public SendMoneyViewModel(AppDbContext context)
        {
            _context = context;
            FromAccountNames = new ObservableCollection<string>();
            ToAccountNames = new ObservableCollection<string>();
            LoadFromAccountsAsync().ConfigureAwait(false);
        }

        public async Task LoadFromAccountsAsync()
        {
            try
            {
                var names = await _context.Accounts
                    .OrderBy(a => a.Name)
                    .Select(a => a.Name)
                    .ToListAsync();

                FromAccountNames = new ObservableCollection<string>(names);
                OnPropertyChanged(nameof(FromAccountNames));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки счетов: {ex.Message}");
            }
        }

        public async Task LoadToAccountsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(FromSelectedAccountName))
                {
                    ToAccountNames.Clear();
                    OnPropertyChanged(nameof(ToAccountNames));
                    return;
                }

                var names = await _context.Accounts
                    .Where(a => a.Name != FromSelectedAccountName) // Исключаем текущий выбранный счет
                    .OrderBy(a => a.Name)
                    .Select(a => a.Name)
                    .ToListAsync();

                ToAccountNames = new ObservableCollection<string>(names);
                OnPropertyChanged(nameof(ToAccountNames));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки счетов: {ex.Message}");
            }
        }
        public async Task ExecuteTransferAsync()
        {
            if (!ValidateTransfer())
                return;

            try
            {
                var fromAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Name == FromSelectedAccountName);

                var toAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Name == ToSelectedAccountName);

                if (fromAccount == null || toAccount == null)
                {
                    MessageBox.Show("Один из счетов не найден");
                    return;
                }

                if (fromAccount.Balance < Amount)
                {
                    MessageBox.Show("Недостаточно средств на счете отправителя");
                    return;
                }

                fromAccount.Balance -= Amount;
                toAccount.Balance += Amount;
                _context.SaveChanges();

                var transaction1 = new Transaction
                {
                    Operation = "Списание",
                    AccountId = fromAccount.Id,
                    Amount = Amount
                };
                var transaction2 = new Transaction
                {
                    Operation = "Пополнение",
                    AccountId = toAccount.Id,
                    Amount = Amount
                };
                _context.Transactions.Add(transaction1);
                _context.Transactions.Add(transaction2);
                await _context.SaveChangesAsync();

                MessageBox.Show("Перевод выполнен успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переводе: {ex.InnerException?.Message}");
            }
        }

        private bool ValidateTransfer()
        {
            if (string.IsNullOrEmpty(FromSelectedAccountName) ||
                string.IsNullOrEmpty(ToSelectedAccountName))
            {
                MessageBox.Show("Выберите счета отправителя и получателя");
                return false;
            }

            if (Amount <= 0)
            {
                MessageBox.Show("Сумма перевода должна быть больше нуля");
                return false;
            }

            if (FromSelectedAccountName == ToSelectedAccountName)
            {
                MessageBox.Show("Нельзя перевести средства на тот же самый счет");
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}