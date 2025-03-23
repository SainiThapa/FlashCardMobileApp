using FlashCardMobileApp.Views.Admin;
using Xamarin.Forms;
using System;
using FlashCardMobileApp.Services;

namespace FlashCardMobileApp.Views.Admin
{
    public partial class AdminHomePage : ContentPage
    {
        private readonly ApiService _apiService;
        public AdminHomePage()
        {
            InitializeComponent();
            _apiService = new ApiService();

        }

        private async void OnViewUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminUsersPage());
        }

        private async void OnViewCategoriesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminCategoriesPage());
        }

        private async void OnResetPasswordsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminResetPasswordsPage());
        }

        private async void OnDownloadUserFlashcardsSummaryClicked(object sender, EventArgs e)
        {
            bool isDownloaded = await _apiService.DownloadUserFlashCardsSummaryAsync();
            if (isDownloaded)
            {
                await DisplayAlert("Download", "User Flashcards Summary downloaded successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to download User Flashcards Summary.", "OK");
            }
        }

        private async void OnDownloadAllFlashcardsWithOwnersClicked(object sender, EventArgs e)
        {
            bool isDownloaded = await _apiService.DownloadAllFlashCardsWithOwnersAsync();
            if (isDownloaded)
            {
                await DisplayAlert("Download", "All Flashcards with Owners downloaded successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to download All Flashcards with Owners.", "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
