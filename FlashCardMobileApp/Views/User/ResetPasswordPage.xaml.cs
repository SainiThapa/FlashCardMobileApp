using System;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.User
{
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Check if passwords match
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "Please enter both fields.", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var email = await App.AuthService.GetUserEmailAsync();
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "User email not found. Please log in again.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var success = await App.ApiService.UpdatePasswordAsync(email, newPassword);
            if (success)
            {
                await DisplayAlert("Success", "Password updated successfully!", "OK");
                await Shell.Current.GoToAsync(".."); // Navigate back
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password. Try again.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
