using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class RandomQuizViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private Flashcard _currentFlashcard;
        private bool _isAnswerVisible;

        public Flashcard CurrentFlashcard
        {
            get => _currentFlashcard;
            set => SetProperty(ref _currentFlashcard, value);
        }

        public bool IsAnswerVisible
        {
            get => _isAnswerVisible;
            set => SetProperty(ref _isAnswerVisible, value);
        }

        public ObservableCollection<Category> Categories { get; }
        public Category SelectedCategory { get; set; }

        public ICommand GetNewFlashcardCommand { get; }
        public ICommand RevealAnswerCommand { get; }

        public RandomQuizViewModel()
        {
            _apiService = new ApiService();
            Categories = new ObservableCollection<Category>();

            GetNewFlashcardCommand = new Command(async () => await LoadRandomFlashcard());
            RevealAnswerCommand = new Command(() => IsAnswerVisible = true);

            LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _apiService.GetCategoriesAsync();
            if (categories != null)
            {
                foreach (var category in categories)
                    Categories.Add(category);
            }
        }

        public async Task LoadRandomFlashcard()
        {
            IsBusy = true;
            IsAnswerVisible = false;

            try
            {
                var flashcard = await _apiService.GetRandomFlashcardAsync(SelectedCategory?.Id);
                if (flashcard != null)
                {
                    CurrentFlashcard = flashcard;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No flashcards found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error loading quiz: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
