using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashCardMobileApp.Services; 
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminHomeViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public int TotalUsers { get; set; }
        public int TotalFlashcards { get; set; }
        public int TotalCategories { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand ViewUsersCommand { get; }

        public AdminHomeViewModel()
        {
            _apiService = new ApiService();  // Assuming you have a service for API calls

            LogoutCommand = new Command(Logout);
            ViewUsersCommand = new Command(async () => await ViewUsersAsync());
            LoadData();
        }

        private async void LoadData()
        {
            var token = await SecureStorage.GetAsync("admin_auth_token");
            Debug.WriteLine("ADMIN AUTH TOKEN IS " + token);
            var users = await _apiService.GetUsersAsync();
            TotalUsers = users.Count;
            Debug.WriteLine("LOADING THE DATA OF THE SYSTEM");
            var categories = await _apiService.GetCategoriesAsync();
            TotalCategories = categories?.Count ?? 0;

            // You can similarly load the total flashcards if needed
            // For now, we're assuming you have a method for flashcards or can calculate from another API call

            OnPropertyChanged(nameof(TotalUsers));
            OnPropertyChanged(nameof(TotalCategories));
        }

        private async void Logout()
        {
            await SecureStorage.SetAsync("admin_auth_token", string.Empty);
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async Task ViewUsersAsync()
        {
            Debug.WriteLine("VIEW USER ASYNC CALLED");
            var users = await _apiService.GetUsersAsync();
            if (users.Any())
            {
                await Shell.Current.GoToAsync("///AdminUsersPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("No users found", "There are no users available.", "OK");
            }
        }
    }
}
