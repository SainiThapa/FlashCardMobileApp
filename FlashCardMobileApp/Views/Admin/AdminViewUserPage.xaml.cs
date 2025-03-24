using FlashCardMobileApp.Services;
using FlashCardMobileApp.ViewModels.Admin;
using System;
using Xamarin.Forms;
using System.Diagnostics;
using FlashCardMobileApp.Views.Admin;
using FlashCardMobileApp.ViewModels;

namespace FlashCardMobileApp.Views.Admin
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class AdminViewUserPage : ContentPage
    {
        private readonly UserDetailViewModel _viewModel;
        private readonly ApiService _apiService;
        private bool _isFirstLoad = true;

        public string UserId { get; set; }

        public AdminViewUserPage(string userId)
        {
            InitializeComponent();
            UserId = userId;
            _viewModel = new UserDetailViewModel();
            _apiService = new ApiService();
            BindingContext = _viewModel;
            MessagingCenter.Subscribe<EditFlashcardViewModel>(this, "FlashcardUpdated", async (sender) =>
            {
                await _viewModel.LoadUserDetails(UserId); 
            });
            MessagingCenter.Subscribe<CreateUserCardViewModel>(this, "FlashcardCreated", async (sender) =>
            {
                await _viewModel.LoadUserDetails(UserId); 
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_isFirstLoad)
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    Debug.WriteLine($"Received UserId: {UserId}");
                    await _viewModel.LoadUserDetails(UserId);
                    _isFirstLoad = false; 
                }
                else
                {
                    Debug.WriteLine("UserId is null or empty.");
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
            MessagingCenter.Unsubscribe<EditFlashcardViewModel>(this, "FlashcardUpdated");
            MessagingCenter.Unsubscribe<CreateUserCardViewModel>(this, "FlashcardCreated");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error : {ex.Message}");
            }
        }

        // Navigate to CreateUserCardPage
        private async void OnCreateFlashcardClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateUserCardPage(UserId));
        }

        // Navigate to EditUserCardPage
        private async void OnEditFlashcardClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is UserFlashcardViewModel flashcard)
            {
                await Navigation.PushAsync(new EditUserCardPage(UserId, flashcard.Id));
            }
        }

        private bool _isDeleteInProgress = false;

        // Delete Flashcard using ApiService
        private async void OnDeleteFlashcardClicked(object sender, EventArgs e)
        {

            if (_isDeleteInProgress)
                return;

            if (sender is Button button && button.CommandParameter is UserFlashcardViewModel flashcard)
            {
                _isDeleteInProgress = true;
                bool confirm = await DisplayAlert("Delete Flashcard", "Are you sure you want to delete this flashcard?", "Yes", "No");
                if (!confirm)
                {
                    _isDeleteInProgress = false;
                    return;
                }

                var success = await _apiService.DeleteFlashcardAsync(UserId, flashcard.Id);
                if (success)
                {
                    await DisplayAlert("Success", "Flashcard deleted successfully.", "OK");
                    _viewModel.LoadUserDetails(UserId);
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete flashcard.", "OK");
                }
                _isDeleteInProgress = false;
            }
        }
    }
}
