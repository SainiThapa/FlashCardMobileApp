using System.Windows.Input;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class NewItemViewModel
    {
        private readonly ApiService _apiService;

        public Flashcard Flashcard { get; set; } = new Flashcard();
        public ICommand SaveCommand { get; }

        public NewItemViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                await _apiService.AddFlashcardAsync(Flashcard);
                await Shell.Current.GoToAsync("//DashboardPage");
            });
        }
    }
}