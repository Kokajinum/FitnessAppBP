using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitnessApp01.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationSettingsPage : ContentPage
    {
        public RegistrationSettingsPage()
        {
            InitializeComponent();
            //ObservableCollection<string> obs = new ObservableCollection<string>
            //{
            //    "1",
            //    "2",
            //    "3",
            //    "4",
            //    "5",
            //    "6",
            //    "7"
            //};
            //MyCarouselView.ItemsSource = obs;
        }
    }
}