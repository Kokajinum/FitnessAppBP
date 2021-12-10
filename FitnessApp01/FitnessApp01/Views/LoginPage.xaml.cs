using FitnessApp01.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (AuthBase.IsAuthenticated())
            {
                await Shell.Current.GoToAsync("//main-content");
            }
        }
    }
}