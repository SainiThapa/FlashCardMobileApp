using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.Flashcards
{
    public partial class YourFlashCardsPage : ContentPage
    {
        private readonly YourFlashCardsViewModel _viewModel;

        public YourFlashCardsPage()
        {
            InitializeComponent();
            _viewModel = new YourFlashCardsViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadFlashcardsCommand.Execute(null);
        }
    }
}
