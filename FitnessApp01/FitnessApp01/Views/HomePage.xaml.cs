
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            //var vm = new ViewModels.HomePageViewModel();
            var vm = BindingContext as ViewModels.HomePageViewModel;
            vm.InitializeViewModel.Execute(null);
            //MessagingCenter.Subscribe<object>(this, "signedOut", (p) =>
            //{
            //    OnSignedOutReceived();
            //});
        }

        //private void OnSignedOutReceived()
        //{
        //    WasSignedOut = true;
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (WasSignedOut)
        //    {
        //        (BindingContext as ViewModels.HomePageViewModel).
        //            InitializeViewModel.Execute(null);
        //        WasSignedOut = false;
        //    }
        //}

        //private bool WasSignedOut = false;
    }
}