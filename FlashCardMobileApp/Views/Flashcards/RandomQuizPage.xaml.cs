using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCardMobileApp.Views.Flashcards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RandomQuizPage : ContentPage
    {
        private readonly RandomQuizViewModel _viewModel;

        public RandomQuizPage()
        {
            InitializeComponent();
            _viewModel = new RandomQuizViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadRandomFlashcard();
        }
    }
}
