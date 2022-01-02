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
                CanSaveChanged();
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
            set 
            { 
                SetProperty(ref _kcalInput, value);
                CanSaveChanged(); 
            }
        }

        private double? _carbsInput;
        public double? CarbsInput
        {
            get { return _carbsInput; }
            set { SetProperty(ref _carbsInput, value); CanSaveChanged(); }
        }

        private double? _sugarInput;
        public double? SugarInput
        {
            get { return _sugarInput; }
            set { SetProperty(ref _sugarInput, value); CanSaveChanged(); }
        }

        private double? _proteinInput;
        public double? ProteinInput
        {
            get { return _proteinInput; }
            set { SetProperty(ref _proteinInput, value); CanSaveChanged(); }
        }

        private double? _fatInput;
        public double? FatInput
        {
            get { return _fatInput; }
            set { SetProperty(ref _fatInput, value); CanSaveChanged(); }
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

        public bool CanSave
        {
            get
            {
                return CheckNameInput() && KcalInput > 0 && CarbsInput > 0 &&
                     SugarInput > 0 && ProteinInput > 0 && FatInput > 0;
            }
        }

        private bool CheckNameInput()
        {
            return !string.IsNullOrEmpty(NameInput)
                && !NameInput.Contains("#")
                && !NameInput.Contains("&");
        }

        private double _saveButtonOpacity = 0.2;
        public double SaveButtonOpacity
        {
            get { return _saveButtonOpacity; }
            set => SetProperty(ref _saveButtonOpacity, value);
        }

        private void CanSaveChanged()
        {
            if (CanSave)
            {
                SaveButtonOpacity = 1;
            }
            else
            {
                SaveButtonOpacity = 0.2;
            }
        }
    }
}
