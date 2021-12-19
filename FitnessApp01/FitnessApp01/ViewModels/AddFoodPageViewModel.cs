using FitnessApp01.Interfaces;
using FitnessApp01.Models;
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
                execute: async () => await SaveFood(),
                canExecute: () => SaveCanExecute());
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

        private bool SaveCanExecute()
        {
            IsVisible = !CanSave;
            return CanSave;
        }

        #region Properties

        //private IDatabase FirestoreBase { get; set; }

        //z nejakeho duvodu nefunguje
        private AddFoodPageAttributes _addFoodPageAttributes;
        public AddFoodPageAttributes Attrs
        {
            get { return _addFoodPageAttributes; }
            set 
            { 
                SetProperty(ref _addFoodPageAttributes, value); 
                OnPropertyChanged("CanSave"); }
        }

        /*
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
        */

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
                return CheckNameInput() && Attrs.KcalInput > 0 && Attrs.CarbsInput >= 0 &&
                     Attrs.SugarInput >= 0 && Attrs.ProteinInput >= 0 && Attrs.FatInput >= 0;
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
            return !string.IsNullOrEmpty(Attrs.NameInput)
                && !Attrs.NameInput.Contains("#")
                && !Attrs.NameInput.Contains("&");
        }

        #endregion

        #region Commands

        public ICommand SaveFoodCommand { get; set; }

        #endregion
    }
}
