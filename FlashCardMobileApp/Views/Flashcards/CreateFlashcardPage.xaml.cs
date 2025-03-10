using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.Flashcards
{
    public partial class CreateFlashcardPage : ContentPage
    {
        private readonly CreateFlashcardViewModel _viewModel;

        public CreateFlashcardPage()
        {
            InitializeComponent();
            _viewModel = new CreateFlashcardViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadCategories();
        }
    }
}
