using FlashCardMobileApp.ViewModels.Admin;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.Admin
{
    public partial class EditUserCardPage : ContentPage
    {
        private readonly EditUserCardViewModel _viewModel;

        public EditUserCardPage(string userId, int flashcardId)
        {
            InitializeComponent();
            _viewModel = new EditUserCardViewModel();
            BindingContext = _viewModel;

            // Load the flashcard details for this user and flashcard
            _viewModel.LoadFlashcardDetails(userId, flashcardId);
        }
    }
}
