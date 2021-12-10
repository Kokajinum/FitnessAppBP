using FitnessApp01.Views;
using Xamarin.Forms;

namespace FitnessApp01
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("SelectMealPage", typeof(SelectMealPage));
            Routing.RegisterRoute("AddFoodPage", typeof(AddFoodPage));
            Routing.RegisterRoute("AddMealPage", typeof(AddMealPage));
            Routing.RegisterRoute("EditMealPage", typeof(EditMealPage));
        }

    }
}
