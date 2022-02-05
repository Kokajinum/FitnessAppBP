using System;
using System.Collections.Generic;
using System.Linq;
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

        private string _brandInput = String.Empty;
        public string BrandInput
        {
            get { return _brandInput; }
            set { SetProperty(ref _brandInput, value); CanSaveChanged(); }
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
            set { SetProperty(ref _saturatedInput, value); CanSaveChanged(); }
        }

        private double? _fiberInput;
        public double? FiberInput
        {
            get { return _fiberInput; }
            set { SetProperty(ref _fiberInput, value); CanSaveChanged(); }
        }

        private double? _saltInput;
        public double? SaltInput
        {
            get { return _saltInput; }
            set { SetProperty(ref _saltInput, value); CanSaveChanged(); }
        }

        private double? _portionSize;
        public double? PortionSize
        {
            get { return _portionSize; }
            set { SetProperty(ref _portionSize, value); CanSaveChanged(); }
        }

        

        private string _portionSizeString;
        public string PortionSizeString
        {
            get { return _portionSizeString; }
            set { SetProperty(ref _portionSizeString, value); }
        }

        #region warning texts

        private bool _isNoReqNutrientsWarningVisible;
        public bool IsNoReqNutrientsWarningVisible
        {
            get { return _isNoReqNutrientsWarningVisible; }
            set { SetProperty(ref _isNoReqNutrientsWarningVisible, value); }
        }

        private bool _isNoEnergyVisible;
        public bool IsNoEnergyVisible
        {
            get { return _isNoEnergyVisible; }
            set { SetProperty(ref _isNoEnergyVisible, value); }
        }

        private bool _isWrongReqSumVisible;
        public bool IsWrongReqSumVisible
        {
            get { return _isWrongReqSumVisible; }
            set { SetProperty(ref _isWrongReqSumVisible, value); }
        }

        private bool _isWrongOptSumVisible;
        public bool IsWrongOptSumVisible
        {
            get { return _isWrongOptSumVisible; }
            set { SetProperty(ref _isWrongOptSumVisible, value);}
        }

        private bool _isWrongSaturatedVisible;
        public bool IsWrongSaturatedVisible
        {
            get { return _isWrongSaturatedVisible; }
            set { SetProperty(ref _isWrongSaturatedVisible, value); }
        }

        private bool _isWrongSugarVisible;
        public bool IsWrongSugarVisible
        {
            get { return _isWrongSugarVisible; }
            set { SetProperty(ref _isWrongSugarVisible, value); }
        }

        private bool _isWrongFiberVisible;
        public bool IsWrongFiberVisible
        {
            get { return _isWrongFiberVisible; }
            set { SetProperty(ref _isWrongFiberVisible, value); }
        }

        private bool _isWrongNameVisible;
        public bool IsWrongNameVisible
        {
            get { return _isWrongNameVisible; }
            set { SetProperty(ref _isWrongNameVisible, value); }
        }


        #endregion

        public bool CanSave
        {
            get
            {
                var check1 = CheckNameInput();
                var check2 = CheckRequiredFieldsInput();
                var check3 = CheckOptionalFieldsInput();
                var check4 = CheckNutrientsSizesInput();
                return check1 && check2 && check3 && check4;
                //return CheckNameInput() && KcalInput > 0 && CarbsInput >= 0 
                //    && ProteinInput >= 0 && FatInput >= 0 && CheckOptional()
                //     && CheckNutrients();
            }
        }

        private bool CheckNameInput()
        {
            bool result = true;
            var isNullOrWhiteSpace = string.IsNullOrWhiteSpace(NameInput);
            if (isNullOrWhiteSpace)
            {
                IsWrongNameVisible = true;
                result = false;
            }
            else
            {
                IsWrongNameVisible = false;
            }

            //if (!string.IsNullOrEmpty(BrandInput))
            //{
            //    bool brandWhiteSpaced = BrandInput.All(x => char.IsWhiteSpace(x));
            //    if (brandWhiteSpaced)
            //    {
            //        BrandInput = null;
            //    }
            //}
            
            return result;
        }

        private bool CheckRequiredFieldsInput()
        {
            var reqNutrientsFilled = CarbsInput >= 0
                && ProteinInput >= 0 && FatInput >= 0;
            var energyFilled = KcalInput > 0;
            if (!reqNutrientsFilled)
            {
                IsNoReqNutrientsWarningVisible = true;
            }
            else
            {
                IsNoReqNutrientsWarningVisible = false;
            }
            if (!energyFilled)
            {
                IsNoEnergyVisible = true;
            }
            else
            {
                IsNoEnergyVisible = false;
            }
            return reqNutrientsFilled;
        }

        private bool CheckOptionalFieldsInput()
        {
            // pokud uživatel zadá třeba do vstupu k FiberInput hodnotu: -, tak to converter převede na 0
            return !(SaturatedInput < 0) && !(FiberInput < 0)
                && !(SaltInput < 0) && !(PortionSize < 0) && !(SugarInput < 0);
        }

        private bool CheckNutrientsSizesInput()
        {
            bool result = true;
            if ((FatInput + CarbsInput + ProteinInput + SaltInput) > 100)
            {
                result = false;
                IsWrongReqSumVisible = true;
            }
            else
            {
                IsWrongReqSumVisible = false;
            }
            if (FatInput >= 0)
            {
                if (SaturatedInput > FatInput)
                {
                    result = false;
                    IsWrongSaturatedVisible = true;
                }
                else
                {
                    IsWrongSaturatedVisible = false;
                }
            }
            if (CarbsInput >= 0)
            {
                if (FiberInput > CarbsInput)
                {
                    result = false;
                    IsWrongFiberVisible = true;
                }
                else
                {
                    IsWrongFiberVisible = false;
                }
                if (SugarInput > CarbsInput)
                {
                    result = false;
                    IsWrongSugarVisible = true;
                }
                else
                {
                    IsWrongSugarVisible = false;
                }
            }
            
            return result;
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
                //IsWrongReqSumVisible = false;
                //IsNoReqNutrientsWarningVisible = false;
            }
            else
            {
                SaveButtonOpacity = 0.2;
            }
        }
    }
}
