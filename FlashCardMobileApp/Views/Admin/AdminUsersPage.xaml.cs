using FlashCardMobileApp.ViewModels;
using FlashCardMobileApp.ViewModels.Admin;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminUsersPage : ContentPage
    {
        private readonly AdminUsersViewModel viewModel;

        public AdminUsersPage()
        {
            InitializeComponent();
            viewModel = new AdminUsersViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadUsersCommand.Execute(null);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is UserViewModel selectedUser)
            {
                Application.Current.MainPage.Navigation.PushAsync(new AdminViewUserPage(selectedUser.Id));
            }
            ((ListView)sender).SelectedItem = null;
        }

        private void OnSelectAllCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Updates SelectAll property in the ViewModel
            viewModel.SelectAll = e.Value;
        }

        private async void OnBulkDeleteClicked(object sender, EventArgs e)
        {
            bool success = await viewModel.BulkDeleteUsers();
            if (success)
            {
                await DisplayAlert("Success", "Users deleted successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete users.", "OK");
            }
        }

        private void CreateUserButton_Clicked(object sender, EventArgs e)
        {
            CreateUserFrame.IsVisible = true;
        }

        private async void OnCreateUserConfirmed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
                string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
                string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            bool isRegistered = await viewModel.RegisterUserAsync(
                EmailEntry.Text,
                PasswordEntry.Text,
                FirstNameEntry.Text,
                LastNameEntry.Text
            );

            if (isRegistered)
            {
                await DisplayAlert("Success", "User registered successfully.", "OK");
                CreateUserFrame.IsVisible = false;
                viewModel.LoadUsersCommand.Execute(null); // Refresh user list
            }
            else
            {
                await DisplayAlert("Error", "Failed to register user.", "OK");
            }
        }

        private void OnCancelCreateUser(object sender, EventArgs e)
        {
            CreateUserFrame.IsVisible = false;
        }
        private void DeleteUserButton_Clicked(object sender, System.EventArgs e)
        {
            if (((Button)sender).BindingContext is UserViewModel selectedUser)
            {
                viewModel.DeleteUserCommand.Execute(selectedUser);
            }
        }
    }
}
