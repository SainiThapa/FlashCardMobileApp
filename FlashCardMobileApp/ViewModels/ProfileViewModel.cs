using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private bool _isEditing;

        public UserProfile Profile { get; set; }
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotEditing));
            }
        }
        public bool IsNotEditing => !_isEditing;

        public ICommand ToggleEditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public ProfileViewModel()
        {
            _apiService = new ApiService();
            Profile = new UserProfile();
            LoadUserProfile();

            ToggleEditCommand = new Command(() => IsEditing = true);
            SaveCommand = new Command(async () => await SaveProfile());
            CancelCommand = new Command(() => IsEditing = false);
            ResetPasswordCommand = new Command(async () => await ResetPassword());
        }

        public async Task LoadUserProfile()
        {
            Profile = await _apiService.GetUserProfileAsync();
            OnPropertyChanged(nameof(Profile));
        }

        private async Task SaveProfile()
        {
            var success = await _apiService.UpdateUserProfileAsync(Profile);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully!", "OK");
                IsEditing = false;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update profile.", "OK");
            }
        }

        private async Task ResetPassword()
        {
            await Shell.Current.GoToAsync("//ResetPasswordPage");
        }
    }
}
