using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FeatureHubPurple.Models
{


    public class TimerItem : INotifyPropertyChanged
    {

        public Guid Id { get; set; } = Guid.NewGuid();


        private int _index;
        public int Index
        {
                get => _index;
                set => SetProperty(ref _index, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _startTime;
        public string StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public bool IsPaused
        {
            get; set;
        }

        private string _endTime;
        public string EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }

        private string _duration;
        public string Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private int _totalMinutes;
        public int TotalMinutes
        {
            get => _totalMinutes;
            set => SetProperty(ref _totalMinutes, value);
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}
