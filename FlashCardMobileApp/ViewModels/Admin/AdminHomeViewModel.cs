using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminHomeViewModel : BaseViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalFlashcards { get; set; }
        public int TotalCategories { get; set; }

        public ICommand LogoutCommand { get; }

        public AdminHomeViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        private async void Logout()
        {
            await SecureStorage.SetAsync("admin_auth_token", string.Empty);
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
