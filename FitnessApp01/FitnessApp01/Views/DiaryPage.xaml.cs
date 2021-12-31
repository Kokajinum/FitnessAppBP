
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
        }
    }
}