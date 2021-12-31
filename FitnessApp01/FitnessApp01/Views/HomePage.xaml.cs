
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
        }
    }
}