using FitnessApp01.Models;
using FitnessApp01.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class AddFoodPageViewModel : BaseViewModel
    {
        public AddFoodPageViewModel()
        {
            SaveFoodCommand = new Command(
                execute: async () => await SaveFood(),
                canExecute: () => SaveCanExecute());
            InitializeAddFoodPageViewModel();
        }

        private void InitializeAddFoodPageViewModel()
        {
            PickerUnits = new ObservableCollection<string>()
            {
                { "g" },
                { "ml" }
            };
            PickerCurrentUnit = PickerUnits[0];
        }

        private async Task SaveFood()
        {
            var newFood = new Food(NameInput, (int)KcalInput, (double)CarbsInput, (double)SugarInput, (double)ProteinInput, (double)FatInput, 
                AuthBase.GetUserId(), PickerCurrentUnit, (double)SaturatedInput, (double)FiberInput, (double)SaltInput, BrandInput, (double)PortionSize);
            var saved = await FirestoreBase.SaveFoodData(newFood);
            if (!saved)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Nepodařilo se uložit data", "ok");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Done", "Podařilo se", "ok");
                await Shell.Current.GoToAsync("..");
            }
        }

        private bool SaveCanExecute()
        {
            IsVisible = !CanSave;
            return CanSave;
        }

        #region Properties

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

        private ObservableCollection<string> _pickerUnits;
        public ObservableCollection<string> PickerUnits
        {
            get { return _pickerUnits; }
            set { SetProperty(ref _pickerUnits, value); }
        }
        
        public string PickerCurrentUnit { get; set; }

        public bool CanSave
        {
            get
            {
                return CheckNameInput() && KcalInput > 0 && CarbsInput >= 0 &&
                     SugarInput >= 0 && ProteinInput >= 0 && FatInput >= 0;
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                SetProperty(ref _isVisible, value);
            }
        }

        private bool CheckNameInput()
        {
            return !string.IsNullOrEmpty(NameInput)
                && !NameInput.Contains("#")
                && !NameInput.Contains("&");
        }

        #endregion

        #region Commands

        public ICommand SaveFoodCommand { get; set; }

        #endregion
    }
}
