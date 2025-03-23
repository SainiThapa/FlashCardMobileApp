using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Services;
using FlashCardMobileApp.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminUsersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<UserViewModel> _users;
        private readonly ApiService _apiService;
        private readonly AuthenticationService _authService;
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ICommand LoadUsersCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand BulkDeleteCommand { get; }

        private bool _selectAll;

        public bool SelectAll
        {
            get => _selectAll;
            set
            {
                if (_selectAll != value)
                {
                    _selectAll = value;
                    OnPropertyChanged();
                    // Update all users' IsSelected property based on SelectAll
                    foreach (var user in Users)
                    {
                        user.IsSelected = _selectAll;
                    }
                }
            }
        }

        public AdminUsersViewModel()
        {
            _apiService = new ApiService();
            _authService= new AuthenticationService();

            Users = new ObservableCollection<UserViewModel>();
            LoadUsersCommand = new Command(async () => await LoadUsers());
            DeleteUserCommand = new Command<UserViewModel>(async (user) => await DeleteUser(user));
            SelectAllCommand = new Command(OnSelectAllToggled);
            BulkDeleteCommand = new Command(async () => await BulkDeleteUsers());
        }

        private async Task LoadUsers()
        {
            Users.Clear();
            var usersList = await _apiService.GetUsersAsync();
            foreach (var user in usersList)
                Users.Add(user);
        }

        private async Task DeleteUser(UserViewModel user)
        {
            if (user == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete User",
                $"Are you sure you want to delete {user.FirstName} {user.LastName}?",
                "Yes",
                "No"
            );

            if (!confirm) return;

            var success = await _apiService.DeleteUserAsync(user.Id);

            if (success)
            {
                Users.Remove(user);
                await Application.Current.MainPage.DisplayAlert("Success", "User deleted successfully.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete user.", "OK");
            }
        }

        public async Task<bool> BulkDeleteUsers()
        {
            var selectedUsers = Users.Where(u => u.IsSelected).ToList();
            if (!selectedUsers.Any())
            {
                await Application.Current.MainPage.DisplayAlert("No users selected", "Please select users to delete.", "OK");
                return false;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Bulk Delete Users",
                $"Are you sure you want to delete {selectedUsers.Count} selected users?",
                "Yes", "No"
            );

            if (!confirm) return false;
            var listofuserid = selectedUsers.Select(u => u.Id).ToList();
            Debug.WriteLine($"User Id in list : {listofuserid}   Number");
            var success = await _apiService.DeleteUsersAsync(listofuserid);
            if (success)
            {
                // Remove deleted users from the collection
                foreach (var user in selectedUsers)
                {
                    Users.Remove(user);
                }

                await Application.Current.MainPage.DisplayAlert("Success", "Users deleted successfully.", "OK");
                return true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete users.", "OK");
                return false;
            }
        }

        public async Task<bool> RegisterUserAsync(string email, string password, string firstName, string lastName)
        {
            bool isRegistered = await _authService.RegisterAsync(email, password, firstName, lastName);
            return isRegistered;
        }

        private void OnSelectAllToggled()
        {
            // Trigger the SelectAll property when the user toggles the "Select All" checkbox
            SelectAll = !SelectAll;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
