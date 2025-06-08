using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FinTracker.ViewModels.Transactions
{
    public class TransactrionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TransactionItem> _transactions;
        public ObservableCollection<TransactionItem> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        public TransactrionViewModel()
        {
            Transactions = new ObservableCollection<TransactionItem>();
            _ = LoadTransactionsAsync(); // Загрузка данных при создании ViewModel
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsFromDbAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.Transactions
                    .Include(t => t.Account) // Если есть связь с Account
                    .ToListAsync();
            }
        }

        public async Task LoadTransactionsAsync()
        {
            var dbTransactions = await GetTransactionsFromDbAsync();

            Transactions.Clear(); // Очищаем существующие данные

            foreach (var dbItem in dbTransactions)
            {
                Transactions.Add(new TransactionItem
                {
                    Operation = dbItem.Operation,
                    Account = dbItem.Account?.Name ?? "Без счета", // Предполагая, что Account - это навигационное свойство
                    Amount = dbItem.Amount
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TransactionItem : INotifyPropertyChanged
    {
        private string _operation;
        private string _account;
        private decimal _amount;

        public string Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                OnPropertyChanged();
            }
        }

        public string Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}