using FitnessApp01.ViewModels;
using FitnessApp01.Views;
using System;
using System.Collections.Generic;
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
