using System;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Services; // Assuming you have a service for API calls
using FlashCardMobileApp.Models;
using System.Threading.Tasks; // Assuming this is where your models are defined

namespace FlashCardMobileApp.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        // User Input Fields
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NewPassword { get; set; }

        public ICommand ForgotPasswordCommand { get; }

        public ForgotPasswordViewModel()
        {
            _apiService = new ApiService(); // Assuming ApiService is used for API calls
            ForgotPasswordCommand = new Command(async () => await ForgotPasswordAsync());
        }

        private async Task ForgotPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var model = new ForgotPasswordViewModel
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                NewPassword = this.NewPassword
            };

            var success = await _apiService.ForgotPasswordAsync(model);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Password reset successful!", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password reset failed.", "OK");
            }
        }
    }
}
