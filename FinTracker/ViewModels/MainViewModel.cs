using FinTracker.Data;
using FinTracker.ViewModels;
using FinTracker.ViewModels.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace FinTracker
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TargetViewModel TargetVM { get; }
        public AccountViewModel AccountVM { get; }
        public MainViewModel()
        {
            TargetVM = new TargetViewModel();
            AccountVM = new AccountViewModel();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
