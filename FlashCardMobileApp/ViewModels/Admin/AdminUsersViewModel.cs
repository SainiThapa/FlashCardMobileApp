using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminUsersViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public ObservableCollection<User> Users { get; set; }
        public ICommand LoadUsersCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public AdminUsersViewModel()
        {
            Title = "Manage Users";
            _apiService = new ApiService();
            Users = new ObservableCollection<User>();

            LoadUsersCommand = new Command(async () => await LoadUsers());
            DeleteUserCommand = new Command<User>(async (user) => await DeleteUser(user));

            LoadUsersCommand.Execute(null);
        }

        private async Task LoadUsers()
        {
            IsBusy = true;
            try
            {
                var users = await _apiService.GetUsersAsync();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteUser(User user)
        {
            if (user == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Delete user {user.Email}?", "Yes", "No");
            if (!confirm) return;

            await _apiService.DeleteUserAsync(user.Id);
            Users.Remove(user);
        }
    }
}
