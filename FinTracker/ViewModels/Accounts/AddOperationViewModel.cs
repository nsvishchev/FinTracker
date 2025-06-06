using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.ViewModels.Accounts
{
    public class AddOperationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _comboItems = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> ComboItems
        {
            get => _comboItems;
            set
            {
                if (_comboItems != value)
                {
                    _comboItems = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public async Task LoadData()
        {
            using var context = new AppDbContext();

            // Получение списка имен сущностей из базы данных
            var entitiesNames = await context.Accounts.Select(e => e.Name).ToListAsync();

            ComboItems.Clear(); // Очистка текущего списка
            foreach (var name in entitiesNames)
            {
                ComboItems.Add(name);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
