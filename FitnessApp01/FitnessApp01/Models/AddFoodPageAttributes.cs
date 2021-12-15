using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessApp01.Models
{
    public class AddFoodPageAttributes : NotifyModel
    {
        private string _nameInput;
        public string NameInput
        {
            get { return _nameInput; }
            set
            {
                SetProperty(ref _nameInput, value);
                OnPropertyChanged("CanSave");
            }
        }

        private string _brandInput;
        public string BrandInput
        {
            get { return _brandInput; }
            set { SetProperty(ref _brandInput, value); }
        }

        private int? _kcalInput;
        public int? KcalInput
        {
            get { return _kcalInput; }
            set { SetProperty(ref _kcalInput, value); OnPropertyChanged("CanSave"); }
        }

        private double? _carbsInput;
        public double? CarbsInput
        {
            get { return _carbsInput; }
            set { SetProperty(ref _carbsInput, value); OnPropertyChanged("CanSave"); }
        }

        private double? _sugarInput;
        public double? SugarInput
        {
            get { return _sugarInput; }
            set { SetProperty(ref _sugarInput, value); OnPropertyChanged("CanSave"); }
        }

        private double? _proteinInput;
        public double? ProteinInput
        {
            get { return _proteinInput; }
            set { SetProperty(ref _proteinInput, value); OnPropertyChanged("CanSave"); }
        }

        private double? _fatInput;
        public double? FatInput
        {
            get { return _fatInput; }
            set { SetProperty(ref _fatInput, value); OnPropertyChanged("CanSave"); }
        }

        private double? _saturatedInput;
        public double? SaturatedInput
        {
            get { return _saturatedInput; }
            set { SetProperty(ref _saturatedInput, value); }
        }

        private double? _fiberInput;
        public double? FiberInput
        {
            get { return _fiberInput; }
            set { SetProperty(ref _fiberInput, value); }
        }

        private double? _saltInput;
        public double? SaltInput
        {
            get { return _saltInput; }
            set { SetProperty(ref _saltInput, value); }
        }

        private double? _portionSize;
        public double? PortionSize
        {
            get { return _portionSize; }
            set { SetProperty(ref _portionSize, value); }
        }
    }
}
