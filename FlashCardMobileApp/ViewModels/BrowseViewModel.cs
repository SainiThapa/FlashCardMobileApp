using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class BrowseViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private UserProfile _user;
        public ObservableCollection<Flashcard> Flashcards { get; } = new ObservableCollection<Flashcard>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username => _user?.Username ?? "N/A";

        public ICommand BackCommand { get; }

        public BrowseViewModel()
        {
            BackCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
        }

        public async Task OnAppearing()
        {
            var flashcards = await App.ApiService.GetFlashcardsAsync();
            if (flashcards != null)
            {
                Flashcards.Clear();
                foreach (var flashcard in flashcards)
                {
                    Flashcards.Add(flashcard);
                }
            }
        }
    }
}