using System.ComponentModel;
using Xamarin.Forms;
using FlashCardMobileApp.ViewModels;
using FlashCardMobileApp.Models;

namespace FlashCardMobileApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(Flashcard flashcard)
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(flashcard);
        }
    }
}