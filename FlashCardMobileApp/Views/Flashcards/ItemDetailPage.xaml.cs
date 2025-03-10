using FlashCardMobileApp.Models;
using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views.Flashcards
{
    public partial class ItemDetailPage : ContentPage
    {
        public Command EditCommand { get; }

        public ItemDetailPage(Flashcard flashcard)
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(flashcard);
            EditCommand = new Command(async () => await Shell.Current.GoToAsync($"//Flashcards/EditFlashcardPage?flashcardId={flashcard.Id}"));
        }
    }
}