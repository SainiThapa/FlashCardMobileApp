using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Services;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Views.Admin;
using Microsoft.AspNet.Identity;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminUsersViewModel : BaseViewModel
    {
        private ObservableCollection<UserViewModel> _users;

        private readonly ApiService _apiService;
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ICommand LoadUsersCommand { get; }
        public ICommand ViewUserCommand { get; }

        private UserViewModel _selectedUser;

        public UserViewModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    // Unselect all users when a new user is selected
                    foreach (var user in _users)
                    {
                        user.IsSelected = false;
                    }

                    _selectedUser = value;

                    // Mark the selected user
                    if (_selectedUser != null)
                    {
                        _selectedUser.IsSelected = true;
                    }

                    OnPropertyChanged();
                }
            }
        }
        public AdminUsersViewModel()
        {
            _apiService = new ApiService();
            Users = new ObservableCollection<UserViewModel>();
            LoadUsersCommand = new Command(async () => await LoadUsers());
            //ViewUserCommand = new Command<UserViewModel>(async (user) => await ViewUserDetails(user));
        }
        //private async void OnUserTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item is UserViewModel selectedUser)
        //    {
        //await Application.Current.MainPage.Navigation.PushAsync(new AdminViewUserPage(selectedUser.Id));
        //    }
        //    ((ListView)sender).SelectedItem = null;
        //}

        private async Task LoadUsers()
        {
            Users.Clear();
            var usersList = await _apiService.GetUsersAsync();
            foreach (var user in usersList)
                Users.Add(user);
        }

        //private async Task ViewUserDetails(UserViewModel user)
        //{
        //    if (user == null) return;
        //    await Shell.Current.GoToAsync($"AdminViewUserPage?userId={user.Id}");
        //}
    }
}
