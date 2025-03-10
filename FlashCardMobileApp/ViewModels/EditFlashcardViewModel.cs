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
    public class EditFlashcardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public Flashcard Flashcard { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        private Category _selectedCategory;
        private int _flashcardId;

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

        public EditFlashcardViewModel()
        {
            _apiService = new ApiService();
            Flashcard = new Flashcard();
            Categories = new ObservableCollection<Category>();

            SaveCommand = new Command(async () => await UpdateFlashcard());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async Task LoadFlashcardDetails(int flashcardId)
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

                var flashcards = await _apiService.GetFlashcardsAsync();
                if (flashcards == null || !flashcards.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No flashcards found from API.", "OK");
                    return;
                }

                var flashcardToEdit = flashcards?.FirstOrDefault(f => f.Id == flashcardId);
                if (flashcardToEdit == null)
                {
                    string ids = string.Join(", ", flashcards.Select(f => f.Id)); // Get list of IDs returned
                    await Application.Current.MainPage.DisplayAlert("Error", $"Flashcard ID {flashcardId} not found. Available IDs: {ids}", "OK");
                    return;
                }

                Flashcard = flashcardToEdit;

                // Find and select the correct category based on CategoryId
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == flashcardToEdit.CategoryId);

                // Update bindings
                OnPropertyChanged(nameof(Flashcard));
                OnPropertyChanged(nameof(SelectedCategory));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load flashcard: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateFlashcard()
        {
            if (SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            try
            {
                Flashcard.CategoryId = SelectedCategory.Id;

                var success = await _apiService.UpdateFlashcardAsync(Flashcard);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Flashcard updated successfully!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update flashcard.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

    }
}
