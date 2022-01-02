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
            if (!Attrs.CanSave)
            {
                await DisplayErrorAsync(AppResources.CanNotSave);
                return;
            }
            try
            {
                var newFood = new Food(Attrs.NameInput, (int)Attrs.KcalInput, (double)Attrs.CarbsInput, (double)Attrs.SugarInput, (double)Attrs.ProteinInput, (double)Attrs.FatInput,
                AuthBase.GetUserId(), PickerCurrentUnit, (double)Attrs.SaturatedInput, (double)Attrs.FiberInput, (double)Attrs.SaltInput, Attrs.BrandInput, (double)Attrs.PortionSize);
                await FirestoreBase.CreateFoodDataAsync(newFood);
                await DisplayAlertAsync("Done", "Podařilo se", "ok");
                await GoToPageAsync("..");
            }
            catch (Exception)
            {
                await DisplayAlertAsync("Error", "Nepodařilo se uložit data", "ok");
                await GoToPageAsync("..");
            }
        }

        //private bool SaveCanExecute()
        //{
        //    IsVisible = !Attrs.CanSave;
        //    return Attrs.CanSave;
        //}

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
        
        public string PickerCurrentUnit { get; set; }

        //public bool CanSave
        //{
        //    get
        //    {
        //        return CheckNameInput() && Attrs.KcalInput > 0 && Attrs.CarbsInput >= 0 &&
        //             Attrs.SugarInput >= 0 && Attrs.ProteinInput >= 0 && Attrs.FatInput >= 0;
        //    }
        //}
        
        //private bool CheckNameInput()
        //{
        //    return !string.IsNullOrEmpty(Attrs.NameInput)
        //        && !Attrs.NameInput.Contains("#")
        //        && !Attrs.NameInput.Contains("&");
        //}

        //private void CanSaveChanged()
        //{
        //    if (CanSave)
        //    {
        //        SaveButtonOpacity = 1;
        //    }
        //    else
        //    {
        //        SaveButtonOpacity = 0.4;
        //    }
        //}

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
