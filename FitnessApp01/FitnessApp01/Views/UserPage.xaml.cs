
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
            var vm = BindingContext as ViewModels.UserPageViewModel;
            vm.InitializeViewModelCommand.Execute(null);
            MessagingCenter.Subscribe<object>(this, "signedOut", (p) =>
            {
                OnSignedOutReceived();
            });
        }

        private void OnSignedOutReceived()
        {
            WasSignedOut = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (WasSignedOut)
            {
                (BindingContext as ViewModels.UserPageViewModel).
                    InitializeViewModelCommand.Execute(null);
                WasSignedOut = false;
            }
        }

        private bool WasSignedOut = false;
    }
}