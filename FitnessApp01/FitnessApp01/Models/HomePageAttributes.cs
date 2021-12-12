using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp01.Models
{
    public class HomePageAttributes : NotifyModel
    {
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

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private double _carbohydratesCurrent;
        public double CarbohydratesCurrent
        {
            get { return _carbohydratesCurrent; }
            set { SetProperty(ref _carbohydratesCurrent, value); }
        }

        private double _proteinCurrent;
        public double ProteinCurrent
        {
            get { return _proteinCurrent; }
            set { SetProperty(ref _proteinCurrent, value); }
        }

        private double _fatCurrent;
        public double FatCurrent
        {
            get { return _fatCurrent; }
            set { SetProperty(ref _fatCurrent, value); }
        }

        private double _carbohydratesGoal;
        public double CarbohydratesGoal
        {
            get { return _carbohydratesGoal; }
            set { SetProperty(ref _carbohydratesGoal, value); }
        }

        private double _proteinGoal;
        public double ProteinGoal
        {
            get { return _proteinGoal; }
            set { SetProperty(ref _proteinGoal, value); }
        }

        private double _fatGoal;
        public double FatGoal
        {
            get { return _fatGoal; }
            set { SetProperty(ref _fatGoal, value); }
        }

        private double _saturatedFatCurrent;
        public double SaturatedFatCurrent
        {
            get { return _saturatedFatCurrent; }
            set { SetProperty(ref _saturatedFatCurrent, value); }
        }

        private double _sugarCurrent;
        public double SugarCurrent
        {
            get { return _sugarCurrent; }
            set { SetProperty(ref _sugarCurrent, value); }
        }

        private double _fiberCurrent;
        public double FiberCurrent
        {
            get { return _fiberCurrent; }
            set { SetProperty(ref _fiberCurrent, value); }
        }

        private double _saltCurrent;
        public double SaltCurrent
        {
            get { return _saltCurrent; }
            set { SetProperty(ref _saltCurrent, value); }
        }

        private double _carbohydratesMacro;
        public double CarbohydratesMacro
        {
            get { return _carbohydratesMacro; }
            set { SetProperty(ref _carbohydratesMacro, value); }
        }

        private double _proteinMacro;
        public double ProteinMacro
        {
            get { return _proteinMacro; }
            set { SetProperty(ref _proteinMacro, value); }
        }

        private double _fatMacro;
        public double FatMacro
        {
            get { return _fatMacro; }
            set { SetProperty(ref _fatMacro, value); }
        }
    }
}
