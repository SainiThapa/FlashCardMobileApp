using FlashCardMobileApp.Views.Flashcards;
using FlashCardMobileApp.Views.User;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.User
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        // Navigate to My Flashcards
        private async void OnViewFlashcardsClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new YourFlashCardsPage());
        }

        // Navigate to Create New Flashcard
        private async void OnCreateFlashcardsClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateFlashcardPage());
        }

        // Navigate to Random Quiz
        private async void OnRandomQuizClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RandomQuizPage());
        }

        // Navigate to View Profile Page
        private async void OnViewProfileClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        // Navigate to Reset Password Page
        private async void OnResetPasswordClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ResetPasswordPage());
        }

        // Handle Logout
        private async void OnLogoutClicked(object sender, System.EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirm)
            {
                await SecureStorage.SetAsync("admin_auth_token", string.Empty);
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
