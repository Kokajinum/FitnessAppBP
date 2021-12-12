using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace FitnessApp01.Models
{
    public class DiaryPageAttributes : NotifyModel
    {

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private string _nameOfTheDay = String.Empty;
        public string NameOfTheDay
        {
            get { return SelectedDay.Current().ToString("D", Thread.CurrentThread.CurrentCulture); }
            set { SetProperty(ref _nameOfTheDay, value); }
        }

        private int _caloriesGoal;
        public int CaloriesGoal
        {
            get { return _caloriesGoal; }
            set { SetProperty(ref _caloriesGoal, value); }
        }
 

        private int _caloriesCurrent;
        public int CaloriesCurrent
        {
            get { return _caloriesCurrent; }
            set { SetProperty(ref _caloriesCurrent, value); }
        }

        private double _caloriesProgress;
        public double CaloriesProgress
        {
            get { return _caloriesProgress; }
            set { SetProperty(ref _caloriesProgress, value); }
        }
    }
}
