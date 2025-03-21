using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Services;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminResetPasswordsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public ICommand ResetPasswordCommand { get; }

        public string Email { get; set; }
        public string NewPassword { get; set; }

        public AdminResetPasswordsViewModel()
        {
            Title = "Reset User Passwords";
            _apiService = new ApiService();
            ResetPasswordCommand = new Command(async () => await ResetPassword());
        }

        private async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter email and new password.", "OK");
                return;
            }

            var success = await _apiService.UpdatePasswordAsync(Email, NewPassword);
            if (success)
                await Application.Current.MainPage.DisplayAlert("Success", "Password reset successfully.", "OK");
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to reset password.", "OK");
        }
    }
}
