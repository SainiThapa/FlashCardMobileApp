using FlashCardMobileApp.ViewModels;
using FlashCardMobileApp.ViewModels.Admin;
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

        private void CreateUserButton_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Create button clicked", "ok", "cancel");
            //Application.Current.MainPage.Navigation.PushAsync(new AdminViewUserPage(selectedUser.Id));
        }
        private void DeleteUserButton_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Delete button clicked", "ok", "cancel");
        }
    }

}
