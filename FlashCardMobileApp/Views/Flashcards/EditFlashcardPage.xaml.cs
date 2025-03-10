using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.Flashcards
{
    [QueryProperty(nameof(FlashcardId), "flashcardId")] 
    public partial class EditFlashcardPage : ContentPage
    {
        private readonly EditFlashcardViewModel _viewModel;
        public int FlashcardId { get; set; }

        public EditFlashcardPage()
        {
            InitializeComponent();
            _viewModel = new EditFlashcardViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (FlashcardId > 0)
            {
                await _viewModel.LoadFlashcardDetails(FlashcardId);
            }
        }
    }
}
