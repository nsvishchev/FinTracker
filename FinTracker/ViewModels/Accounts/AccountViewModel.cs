using FinTracker.Data;
using FinTracker.ViewModels.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace FinTracker.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private int _currentId = 1;
        private string _name;
        public string Name
        {
            get { return _name; } 
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        public ICommand RelayCommand { get; }
        public ICommand No { get; }
        public AccountViewModel()
        {
            RelayCommand = new RelayCommand(LoadNextAccountAsync);
            No = new RelayCommand(LoadPreviousAccountAsync);
        }
        public async void LoadNextAccountAsync()
        {
            List<Account> accounts = (List<Account>)await GetAccountAsync();
            if (accounts.Count > 1 && _currentId < accounts.Count - 1)
            {
                var nextAccount = accounts[_currentId + 1];
                Name = nextAccount.Name;
                Balance = nextAccount.Balance;
                _currentId++;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Balance));
            }
            else
            {
                MessageBox.Show("Это последний счет!");
            }
        }
        public async void LoadPreviousAccountAsync()
        {
            List<Account> accounts = (List<Account>)await GetAccountAsync();
            if (_currentId > 0)
            {
                var previousAccount = accounts[_currentId - 1];
                Name = previousAccount.Name;
                Balance = previousAccount.Balance;
                _currentId--;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Balance));
            }
            else
            {
                MessageBox.Show("Это первый счет!");
            }
        }
        public async Task AddAccountAsync()
        {
            using (var db = new AppDbContext())
            {
                try
                {
                    var account = new Account
                    {
                        Name = Name,
                        Balance = 0
                    };
                    if (Name != "e") // !!!
                    {
                        await db.AddRangeAsync(account);
                        await db.SaveChangesAsync();

                        MessageBox.Show("Счет успешно добавлен");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Установлено максимальное количество счетов!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        public async Task<IEnumerable<Account>> GetAccountAsync()
        {
            using (var db = new AppDbContext())
            {
                return await db.Accounts.ToListAsync();
            }
        }
        public virtual async Task LoadAccountAsync()
        {

            var accounts = await GetAccountAsync();
            if (accounts.Any())
            {
                Name = accounts.First().Name;
                Balance = accounts.First().Balance;
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
