using FinTracker;
using FinTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace FinTracker
{
    public class TargetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _targetName;
        private decimal _targetAmount;
        private decimal _collectedAmount;

        public string TargetName
        {
            get => _targetName;
            set
            {
                _targetName = value;
                OnPropertyChanged(nameof(TargetName));
            }
        }
        public decimal TargetAmount
        {
            get => _targetAmount;
            set
            {
                _targetAmount = value;
                OnPropertyChanged(nameof(TargetAmount));
            }
        }
        public decimal CollectedAmount
        {
            get => _collectedAmount;
            set
            {
                _collectedAmount = value;
                OnPropertyChanged(nameof(CollectedAmount));
            }
        }
        public async Task AddTargetAsync()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var target = new Target
                    {
                        TargetName = TargetName,
                        TargetAmount = TargetAmount,
                        CollectedAmount = 0
                    };

                    await db.Targets.AddAsync(target);

                    await db.SaveChangesAsync();

                    MessageBox.Show("Цель успешно сохранена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message);
            }
        }

        public async Task LoadTargetAsync()
        {
            var targets = await GetTargetAsync();
            if (targets.Any())
            {
                TargetName = targets.First().TargetName;
                TargetAmount = targets.First().TargetAmount;
                CollectedAmount = targets.First().CollectedAmount;

            }
        }
        public async Task<IEnumerable<Target>> GetTargetAsync()
        {
            using (var context = new AppDbContext())
            {
                return await context.Targets.ToListAsync();
            }

        }

        public async Task DeleteTargetAsync()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    context.Database.ExecuteSqlRaw("DELETE FROM targets");
                    await context.SaveChangesAsync();
                    MessageBox.Show("Все записи удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении записей: {ex.Message}");
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
