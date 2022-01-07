
using FitnessApp01.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPage : ContentPage
    {
        public DiaryPage()
        {
            InitializeComponent();
            var vm = BindingContext as ViewModels.DiaryPageViewModel;
            vm.InitializeViewModel.Execute(null);
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
                (BindingContext as ViewModels.DiaryPageViewModel).InitializeViewModel.Execute(null);
                WasSignedOut = false;
            }
        }

        private bool WasSignedOut = false;

    }
}