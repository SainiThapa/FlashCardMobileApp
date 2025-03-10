using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _welcomeMessage;
        private int _totalFlashcards;

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        public int TotalFlashcards
        {
            get => _totalFlashcards;
            set => SetProperty(ref _totalFlashcards, value);
        }

        public Command LoadDataCommand { get; }

        public WelcomeViewModel()
        {
            _apiService = new ApiService();
            LoadDataCommand = new Command(async () => await LoadDataAsync());

            // Load data when ViewModel is created
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            IsBusy = true;
            try
            {
                Debug.WriteLine("Fetching user profile...");
                var profile = await _apiService.GetUserProfileAsync();

                if (profile != null)
                {
                    Debug.WriteLine($"Profile loaded: {profile.Username}");
                    WelcomeMessage = $"Welcome, {profile.Username}";
                    TotalFlashcards = profile.TaskCount;
                }
                else
                {
                    Debug.WriteLine("Failed to load user profile.");
                    WelcomeMessage = "Welcome, Guest";
                    TotalFlashcards = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching data: {ex.Message}");
                WelcomeMessage = "Welcome, Guest";
                TotalFlashcards = 0;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
