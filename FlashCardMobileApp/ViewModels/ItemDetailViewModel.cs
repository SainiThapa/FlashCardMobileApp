using FlashCardMobileApp.Models;

namespace FlashCardMobileApp.ViewModels
{
    public class ItemDetailViewModel
    {
        public Flashcard Flashcard { get; set; }

        public ItemDetailViewModel(Flashcard flashcard)
        {
            Flashcard = flashcard;
        }
    }
}