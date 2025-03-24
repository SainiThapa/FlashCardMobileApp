using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class CreateUserCardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly string _userId;

        public ObservableCollection<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId => SelectedCategory?.Id ?? 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateUserCardViewModel(string userId)
        {
            _apiService = new ApiService();
            _userId = userId;
            Categories = new ObservableCollection<Category>();

            SaveCommand = new Command(async () => await CreateFlashcardAsync());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            LoadCategories();
        }

        private async Task LoadCategories()
        {
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
        }

        private async Task CreateFlashcardAsync()
        {
            if (SelectedCategory == null || string.IsNullOrWhiteSpace(Question) || string.IsNullOrWhiteSpace(Answer))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            try
            {
                var createModel = new CreateUserCardViewModel(_userId)
                {
                    SelectedCategory = SelectedCategory,
                    Question = Question,
                    Answer = Answer
                };

                var success = await _apiService.CreateFlashcardAsync(_userId, createModel);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Flashcard created successfully!", "OK");
                    MessagingCenter.Send(this, "FlashcardCreated");
                    await Shell.Current.GoToAsync("..");
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
