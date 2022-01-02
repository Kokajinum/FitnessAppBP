using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using FitnessApp01.Interfaces;
using FitnessApp01.Resx;
using FitnessApp01.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FitnessApp01.Droid.Services.AndroidAuth))]
namespace FitnessApp01.Droid.Services
{
    public class AndroidAuth : IAuth
    {
        private readonly FirebaseAuth mAuth = FirebaseAuth.Instance;
        
        private async Task ReAuthenticate(AuthCredential credential)
        {
            try
            {
                await mAuth.CurrentUser.ReauthenticateAsync(credential);
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                throw new Exception(AppResources.BadCredentials, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(AppResources.Reauthenticate_UnexpectedError, ex);
            }
        }

        public async Task UpdatePassword(string oldPassword, string oldPasswordConfirm, string password)
        {
            try
            {
                //if (oldPassword != oldPasswordConfirm)
                //{
                //    throw new Exception(AppResources.PasswordConfirmError);
                //}
                await ReAuthenticate(EmailAuthProvider.GetCredential(GetUserEmail(), oldPassword));
                await mAuth.CurrentUser.UpdatePasswordAsync(password);
            }
            // u některých metod je za určitých podmínek potřeba reauthentizace
            //catch (FirebaseAuthRecentLoginRequiredException)
            //{
            //    await ReAuthenticate(EmailAuthProvider.GetCredential(GetUserEmail(), oldPassword));
            //}
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUserEmail()
        {
            return mAuth.CurrentUser.Email;
        }

        public string GetUserId()
        {
            return mAuth.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return mAuth.CurrentUser != null;
        }

        public async Task LoginUserAsync(string email, string password)
        {
            try
            {
                IAuthResult authResult = await mAuth.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                throw new Exception(AppResources.BadCredentials, ex);
            }

            catch (FirebaseAuthInvalidUserException ex)
            {
                throw new Exception(AppResources.InvalidUsername, ex);
            }
            catch(FirebaseAuthWebException ex)
            {
                throw new Exception(AppResources.InternetRequired, ex);
            }

            catch (Exception ex)
            {
                throw new Exception(AppResources.UnknownError, ex);
            }
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            try
            {
                IAuthResult authResult = await mAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            }
            catch (FirebaseAuthWeakPasswordException ex)
            {
                throw new Exception(AppResources.WeakPasswordException, ex);
            }

            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception("invalid credentials " + e.Message);
            }
            catch (FirebaseAuthUserCollisionException ex)
            {
                throw new Exception(AppResources.UserCollision, ex);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public bool SignOut()
        {
            try
            {
                mAuth.SignOut();
                return true;
            }
            catch (FirebaseAuthException e)
            {
                throw new Exception("firebaseauthexception " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("unknown error " + e.Message);
            }
        }
    }
}