using FlashCardMobileApp.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views
{
    public partial class AdminLoginPage : ContentPage
    {
        private readonly ApiService apiService;
        public AdminLoginPage()
        {
            InitializeComponent();
            apiService = new ApiService();
        }

        private async void OnAdminLoginClicked(object sender, EventArgs e)
        {
            string email = AdminEmailEntry.Text;
            string password = AdminPasswordEntry.Text;

            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Validation Error", "You can't leave the email field empty.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Validation Error", "You can't leave the password field empty.", "OK");
                return;
            }

            bool success = await apiService.AdminLoginAsync(email, password);
            if (success)
            {
                Debug.WriteLine("Sucessful login admin");
                await Shell.Current.GoToAsync("//AdminHomePage");
            }
            else
            {
                await DisplayAlert("Error", "Invalid admin credentials.", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
