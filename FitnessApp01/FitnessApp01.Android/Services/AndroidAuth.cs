using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
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

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                IAuthResult authResult = await mAuth.SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }

            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }

            catch (FirebaseAuthInvalidUserException e)
            {
                throw new Exception(e.Message);
            }

            catch (Exception e)
            {
                throw new Exception("unknown error " + e.Message);
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                IAuthResult authResult = await mAuth.CreateUserWithEmailAndPasswordAsync(email, password);
                /*var profileUpdates = new Firebase.Auth.UserProfileChangeRequest.Builder();
                profileUpdates.SetDisplayName(userName);
                var build = profileUpdates.Build();
                var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
                await user.UpdateProfileAsync(build);*/

                return true;
            }
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(AppResources.WeakPasswordException + e.Message);
            }

            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception("invalid credentials " + e.Message);
            }

            catch (FirebaseAuthUserCollisionException e)
            {
                throw new Exception("user collision" + e.Message);
            }

            catch (Exception e)
            {
                throw new Exception("unknown error " + e.Message);
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