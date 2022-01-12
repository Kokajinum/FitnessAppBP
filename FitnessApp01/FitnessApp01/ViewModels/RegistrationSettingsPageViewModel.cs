using FitnessApp01.Helpers;
using FitnessApp01.Interfaces;
using FitnessApp01.Models;
using FitnessApp01.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class RegistrationSettingsPageViewModel : BaseViewModel
    {
        public RegistrationSettingsPageViewModel()
        {
            CarouselSource = new ObservableCollection<string>()
            { "1","2","3","4","5","6","7" };
            WeightOptionsList = new List<string>()
            { "kg" };
            NextPageCommand = new Command(execute: async () => await NextPage());
            PreviousPageCommand = new Command(execute: () => PreviousPage());
        }

        public void PreviousPage()
        {
            if (CarouselPosition == 0)
            {
                return;
            }
            CarouselPosition--;
        }

        public async Task NextPage()
        {
            switch (CarouselPosition)
            {
                case 0:
                    if (!ValidatePage1())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 1:
                    if (!ValidatePage2())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 2:
                    if (!ValidatePage3())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 3:
                    if (!ValidatePage4())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 4:
                    if (!ValidatePage5())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 5:
                    if (!ValidatePage6())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                case 6:
                    if (!ValidatePage7())
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "chybný vstup", "ok");
                        return;
                    }
                    break;
                default:
                    break;
            }

            if ((CarouselPosition == 5 && GoalsRadioButtonSelected == "3") ||
                CarouselPosition == 6)
            {
                IsBusy = true;
                await SetRegSettingsAndDB();
                //EventAggregator.BroadCast2();
                CarouselPosition = 0;
                IsBusy = false;
                await Shell.Current.GoToAsync("//DiaryPage");

                return;
            }
            CarouselPosition++;
            Console.WriteLine("zmena");
        }



        #region Helpers

        /// <summary>
        /// Vygeneruje RegistrationSettings a uloží do databáze
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SetRegSettingsAndDB()
        {
            RegistrationSettings = GenerateRegistrationSettings();
            try
            {
                await FirestoreBase.CreateRegistrationSettingsAsync(RegistrationSettings);
                return true;
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message + " " + e.InnerException, "ok");
                return false;
            }
        }

        /// <summary>
        /// Vygeneruje RegistrationSettings na základě vstupu od uživatele
        /// </summary>
        /// <returns></returns>
        private RegistrationSettings GenerateRegistrationSettings()
        {
            var rs = new RegistrationSettings
            {
                Email = AuthBase.GetUserEmail(),
                AgeDB = this.AgeDB,
                WeightDB = this.WeightDB,
                HeightDB = this.HeightDB,
                GenderDB = this.GenderDB,
                ActivityDB = this.ActivityDB,
                GoalDB = this.GoalDB,
                DesiredWeightDB = this.DesiredWeightDB,
                WeightMeasureDB = this.WeightMeasureDB,
                DesiredWeightMeasureDB = this.DesiredWeightMeasureDB,
                Macros = GenerateMacros()
            };
            rs.CaloriesGoal = CalorieGoalCalculator.Calculate(rs);
            rs.GoalSpeed = this.GoalDB == 1 || this.GoalDB == 2 ? 0.1 : 1;
            return rs;
        }

        /// <summary>
        /// Vytvoří výchozí hodnoty pro makra
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, int> GenerateMacros()
        {
            return new Dictionary<string, int>
            {
                { "protein", 25 },
                { "carbohydrates", 45 },
                { "fat", 30 }
            };
        }

        private bool ValidatePage7()
        {
            if (!DesiredWeightInput.Equals(string.Empty) &&
                double.TryParse(DesiredWeightInput, out double num) &&
                num > 0)
            {
                DesiredWeightDB = num;
                //DesiredWeightMeasureDB = DesiredWeightPickerIndex == 0 ? "kg" : "lbs";
                DesiredWeightMeasureDB = "kg";
                return true;
            }
            return false;
        }

        private bool ValidatePage6()
        {
            if ((GoalsRadioButtonSelected != string.Empty) &&
                int.TryParse(GoalsRadioButtonSelected, out int num))
            {
                GoalDB = num;
                if (GoalDB == 3) //udrzet soucasnou vahu
                {
                    DesiredWeightDB = WeightDB;
                    DesiredWeightMeasureDB = WeightMeasureDB;
                }
                return true;
            }
            return false;
        }

        private bool ValidatePage5()
        {
            //problém s double cisly 1.1 vs 1,1 (ja pouzivam 1.1 v XAML) -> invariantculture
            if ((ActivityRadioButtonSelected != string.Empty) &&
                double.TryParse(ActivityRadioButtonSelected, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double num))
            {
                ActivityDB = num;
                return true;
            }
            return false;
        }

        private bool ValidatePage4()
        {
            if (GenderRadioButtonSelected != string.Empty)
            {
                GenderDB = GenderRadioButtonSelected;
                return true;
            }
            return false;
        }

        private bool ValidatePage3()
        {
            if (!string.IsNullOrEmpty(HeightInput) &&
                double.TryParse(HeightInput, out double num) &&
                num > 0)
            {
                HeightDB = num;
                return true;
            }
            return false;
        }

        private bool ValidatePage2()
        {
            if (!string.IsNullOrEmpty(WeightInput) &&
                double.TryParse(WeightInput, out double num) &&
                num > 0)
            {
                WeightDB = num;
                //WeightMeasureDB = WeightPickerIndex == 0 ? "kg" : "lbs";
                WeightMeasureDB = "kg";
                return true;
            }
            return false;
        }

        private bool ValidatePage1()
        {
            int num;
            if (int.TryParse(AgeInput, out num) && num > 0)
            {
                AgeDB = num;
                return true;
            }
            return false;
        }

        

        #endregion


        #region Properties

        //public IDatabase FirestoreBase { get; set; }

        private ObservableCollection<string> _carouselSource;
        public ObservableCollection<string> CarouselSource
        {
            get { return _carouselSource; }
            set { SetProperty(ref _carouselSource, value); }
        }

        private int _carouselPosition;
        public int CarouselPosition
        {
            get { return _carouselPosition; }
            set { SetProperty(ref _carouselPosition, value); }
        }

        private string _ageInput = string.Empty;
        public string AgeInput
        {
            get { return _ageInput; }
            set { SetProperty(ref _ageInput, value); }
        }

        private string _weightInput = string.Empty;
        public string WeightInput
        {
            get { return _weightInput; }
            set { SetProperty(ref _weightInput, value); }
        }

        private List<string> _weightOptionsList;
        public List<string> WeightOptionsList
        {
            get { return _weightOptionsList; }
            set { SetProperty(ref _weightOptionsList, value); }
        }

        private int _weightPickerIndex = 0;
        public int WeightPickerIndex
        {
            get { return _weightPickerIndex; }
            set { SetProperty(ref _weightPickerIndex, value); }
        }

        private string _heightInput = string.Empty;
        public string HeightInput
        {
            get { return _heightInput; }
            set { SetProperty(ref _heightInput, value); }
        }

        private string _genderRadioButtonSelected = string.Empty;
        public string GenderRadioButtonSelected
        {
            get { return _genderRadioButtonSelected; }
            set { SetProperty(ref _genderRadioButtonSelected, value); }
        }

        
        private string _activityRadioButtonSelected = string.Empty;
        public string ActivityRadioButtonSelected
        {
            get { return _activityRadioButtonSelected; }
            set { SetProperty(ref _activityRadioButtonSelected, value); }
        }

        private string _goalsRadioButtonSelected = string.Empty;
        public string GoalsRadioButtonSelected
        {
            get { return _goalsRadioButtonSelected; }
            set { SetProperty(ref _goalsRadioButtonSelected, value); }
        }

        private string _desiredWeightInput = string.Empty;
        public string DesiredWeightInput
        {
            get { return _desiredWeightInput; }
            set { SetProperty(ref _desiredWeightInput, value); }
        }

        private int _desiredWeightPickerIndex = 0;
        public int DesiredWeightPickerIndex
        {
            get { return _desiredWeightPickerIndex; }
            set { SetProperty(ref _desiredWeightPickerIndex, value); }
        }

        #region Cloud Firestore

        public RegistrationSettings RegistrationSettings { get;set; }

        private int AgeDB { get; set; }
        private double WeightDB { get; set; }
        private double HeightDB { get; set; }
        private string GenderDB { get; set; }
        private double ActivityDB { get; set; }
        private int GoalDB { get; set; }
        private double DesiredWeightDB { get; set; }
        private string WeightMeasureDB { get; set; }
        private string DesiredWeightMeasureDB { get; set; }

        #endregion

        #endregion

        #region Commands 

        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public ICommand Nothing { get; set; }

        #endregion
    }
}
