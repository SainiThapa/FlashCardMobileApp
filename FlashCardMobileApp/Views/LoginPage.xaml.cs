using System.Windows.Input;
using System;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public ICommand RegisterCommand => new Command(async () => await Shell.Current.GoToAsync("//RegisterPage"));

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                var success = await App.AuthService.LoginAsync(EmailEntry.Text, PasswordEntry.Text);
                if (success)
                    await Shell.Current.GoToAsync("//WelcomePage");
                else
                    await DisplayAlert("Error", "Invalid email or password", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.InnerException?.Message ?? ex.Message, "OK");
                Console.WriteLine($"Error Details: {ex.InnerException?.ToString() ?? ex.ToString()}");
            }
        }
        private async void OnAdminLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage());
        }
    }
}