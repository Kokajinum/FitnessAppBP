using FitnessApp01.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace FitnessApp01.ViewModels
{
    public class UserPageViewModel : BaseViewModel
    {
        public UserPageViewModel()
        {
            SignOutCommand = new Command(SignOut);
            UserEmail = AuthBase.GetUserEmail();
        }

        private async void SignOut(object obj)
        {
            bool result = await AuthBase.SignOut();
            if (result)
            {
                await Shell.Current.GoToAsync("//LoginPage");
                
            }
        }

        #region Properties

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        #endregion

        #region Commands 

        public ICommand SignOutCommand { get; set; }

        #endregion
    }
}
