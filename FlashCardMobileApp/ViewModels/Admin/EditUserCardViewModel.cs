using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class EditUserCardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public Flashcard Flashcard { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        private Category _selectedCategory;
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }

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

        public EditUserCardViewModel()
        {
            _apiService = new ApiService();
            Flashcard = new Flashcard();
            Categories = new ObservableCollection<Category>();

            SaveCommand = new Command(async () => await UpdateFlashcard());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async Task LoadFlashcardDetails(string userId, int flashcardId)
        {
            IsBusy = true;
            try
            {
                // Load the categories
                var categories = await _apiService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                // Load the flashcard
                var flashcard = await _apiService.GetFlashcardByIdAsync(flashcardId);
                if (flashcard != null)
                {
                    Flashcard = flashcard;

                    // Find the selected category
                    SelectedCategory = Categories.FirstOrDefault(c => c.Id == Flashcard.CategoryId);

                    // Update bindings
                    OnPropertyChanged(nameof(Flashcard));
                    OnPropertyChanged(nameof(SelectedCategory));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Flashcard not found.", "OK");
                }
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
                Flashcard.Question = Question;  // Update with the new Question from the UI
                Flashcard.Answer = Answer;      // Update with the new Answer from the UI
                Flashcard.CategoryId = SelectedCategory.Id;

                var model = new UpdateFlashCardViewModel
                {
                    Question = Flashcard.Question,
                    Answer = Flashcard.Answer,
                    CategoryId = Flashcard.CategoryId
                };

                var success = await _apiService.UpdateUserFlashcardAsync(Flashcard.UserId, Flashcard.Id, model);
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
    public class UpdateFlashCardViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }

}
