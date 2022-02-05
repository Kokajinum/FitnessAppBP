using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using FitnessApp01.Resx;
using FitnessApp01.Services;
using System;
using System.Collections.ObjectModel;
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
                execute: async () => await SaveFood());
            //FirestoreBase = DependencyService.Get<IDatabase>();
            
            //FirestoreBase = Services.FirestoreBase.Instance;
            Attrs = new AddFoodPageAttributes();
            InitializeAddFoodPageViewModel();
        }

        public void InitializeAddFoodPageViewModel()
        {
            PickerUnits = new ObservableCollection<string>()
            {
                { "g" },
                { "ml" }
            };
            PickerCurrentUnit = PickerUnits[0];


        }

        public async Task SaveFood()
        {
            if (!Connection.IsConnected)
            {
                await DisplayErrorAsync(AppResources.InternetRequired);
            }
            if (!Attrs.CanSave)
            {
                await DisplayErrorAsync(AppResources.CanNotSave);
                return;
            }
            if (Attrs.KcalInput >= 1000)
            {
                bool result = await App.Current.MainPage.DisplayAlert("Ověření",
                    "Opravdu má potravina " + Attrs.KcalInput.ToString() + "kcal?", AppResources.Yes, AppResources.No);
                if (!result)
                {
                    return;
                }
            }
            TrimNameAndBrand();
            try
            {
                IsBusy = true;
                var newFood = new Food(Attrs.NameInput, (int)Attrs.KcalInput, (double)Attrs.CarbsInput, (double)Attrs.SugarInput, (double)Attrs.ProteinInput, (double)Attrs.FatInput,
                AuthBase.GetUserId(), PickerCurrentUnit, (double)Attrs.SaturatedInput, (double)Attrs.FiberInput, (double)Attrs.SaltInput, Attrs.BrandInput, (double)Attrs.PortionSize);
                await FirestoreBase.CreateFoodDataAsync(newFood);
                IsBusy = false;
                await DisplaySuccessAsync("Podařilo se uložit novou potravinu.");
                await GoToPageAsync("..");

            }
            catch (Exception)
            {
                IsBusy = false;
                await DisplayAlertAsync("Error", "Nepodařilo se uložit data", "ok");
                await GoToPageAsync("..");
            }
            
        }

        private void TrimNameAndBrand()
        {
            if (Attrs.BrandInput == null)
            {
                Attrs.BrandInput = string.Empty;
            }
            Attrs.NameInput = Attrs.NameInput.Trim();
            Attrs.BrandInput = Attrs.BrandInput.Trim();
        }

        #region Properties

        private AddFoodPageAttributes _addFoodPageAttributes;
        public AddFoodPageAttributes Attrs
        {
            get { return _addFoodPageAttributes; }
            set 
            {
                if (_addFoodPageAttributes != value)
                    _addFoodPageAttributes = value;
            }
        }

        private ObservableCollection<string> _pickerUnits;
        public ObservableCollection<string> PickerUnits
        {
            get { return _pickerUnits; }
            set { SetProperty(ref _pickerUnits, value); }
        }
        
        private string _pickerCurrentUnit;
        public string PickerCurrentUnit 
        {
            get { return _pickerCurrentUnit;}
            set 
            { 
                if (SetProperty(ref _pickerCurrentUnit, value))
                {
                    if (Attrs != null)
                    {
                        if (_pickerCurrentUnit == "g")
                            Attrs.PortionSizeString = AppResources.AddFoodPage_Portion;
                        else if (_pickerCurrentUnit == "ml")
                            Attrs.PortionSizeString = AppResources.AddFoodPage_PortionMl;
                    }
                    
                } 
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

        //private double _saveButtonOpacity = 0.2;
        //public double SaveButtonOpacity
        //{
        //    get { return _saveButtonOpacity; }
        //    set => SetProperty(ref _saveButtonOpacity, value);
        //}

        #endregion

        #region Commands

        public ICommand SaveFoodCommand { get; set; }

        #endregion
    }
}
