using FlashCardMobileApp.Views.Flashcards;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.User
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void OnViewFlashcardsClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new YourFlashCardsPage());
        }

        private async void OnCreateFlashcardsClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreateFlashcardPage());
        }

        private async void OnRandomQuizClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RandomQuizPage());
        }
    }
}
