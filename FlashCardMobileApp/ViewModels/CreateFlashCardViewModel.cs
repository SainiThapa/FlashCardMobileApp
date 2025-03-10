using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class CreateFlashcardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public Flashcard Flashcard { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateFlashcardViewModel()
        {
            _apiService = new ApiService();
            Flashcard = new Flashcard();
            Categories = new ObservableCollection<Category>();

            SaveCommand = new Command(async () => await SaveFlashcard());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async Task LoadCategories()
        {
            IsBusy = true;
            try
            {
                var categories = await _apiService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveFlashcard()
        {
            if (SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            try
            {
                var userId = await _apiService.GetUserIdAsync();
                var flashcardToSave = new
                {
                    CategoryId = SelectedCategory.Id,
                    Question = Flashcard.Question,
                    Answer = Flashcard.Answer,
                    UserId = userId
                };

                var success = await _apiService.AddFlashcardAsync(flashcardToSave);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Flashcard created successfully!", "OK");
                    await Shell.Current.GoToAsync("//YourFlashCardsPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to create flashcard.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
