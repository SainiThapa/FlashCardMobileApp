using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using FlashCardMobileApp.Views.Flashcards;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels
{
    public class YourFlashCardsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Flashcard> Flashcards { get; set; }
        public ObservableCollection<Flashcard> SelectedFlashcards { get; set; }

        public ICommand LoadFlashcardsCommand { get; }
        public ICommand DeleteFlashcardCommand { get; }
        public ICommand EditFlashcardCommand { get; }
        public ICommand BulkDeleteFlashcardsCommand { get; }
        public ICommand NavigateToCreateCommand { get; }

        public YourFlashCardsViewModel()
        {   
            _apiService = new ApiService();
            Flashcards = new ObservableCollection<Flashcard>();
            SelectedFlashcards = new ObservableCollection<Flashcard>();

            LoadFlashcardsCommand = new Command(async () => await LoadFlashcards());
            DeleteFlashcardCommand = new Command<Flashcard>(async (flashcard) => await DeleteFlashcard(flashcard));
            EditFlashcardCommand = new Command<Flashcard>(async (flashcard) => await EditFlashcard(flashcard));
            BulkDeleteFlashcardsCommand = new Command(async () => await BulkDeleteFlashcards());
            NavigateToCreateCommand = new Command(async () => await NavigateToCreatePage());
        }

        private async Task LoadFlashcards()
        {
            IsBusy = true;
            try
            {
                var flashcards = await _apiService.GetFlashcardsAsync();
                Flashcards.Clear();
                if (flashcards != null)
                {
                    foreach (var flashcard in flashcards)
                    {
                        Flashcards.Add(flashcard);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No flashcards found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load flashcards: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteFlashcard(Flashcard flashcard)
        {
            if (flashcard == null) return;

            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this flashcard?", "Yes", "No");
            if (!confirm) return;

            try
            {
                var success = await _apiService.DeleteFlashcardAsync(flashcard.Id);
                if (success)
                {
                    Flashcards.Remove(flashcard);
                    await Application.Current.MainPage.DisplayAlert("Success", "Flashcard deleted successfully!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete flashcard.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error deleting flashcard: {ex.Message}", "OK");
            }
        }

        private async Task BulkDeleteFlashcards()
        {
            var selectedFlashcards = Flashcards.Where(f => f.IsSelected).ToList();
            if (!selectedFlashcards.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No flashcards selected.", "OK");
                return;
            }
            Debug.WriteLine("BulkDeleteFlashcards: Selected IDs - " + string.Join(", ", selectedFlashcards.Select(f => f.Id)));

            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", $"Delete {selectedFlashcards.Count} selected flashcards?", "Yes", "No");
            if (!confirm) return;

            try
            {
                var flashcardIds = selectedFlashcards.Select(f => f.Id).ToList();
                var success = await _apiService.BulkDeleteFlashcardsAsync(flashcardIds);

                if (success)
                {
                    Debug.WriteLine("BulkDeleteFlashcards: Successfully deleted flashcards.");

                    foreach (var flashcard in selectedFlashcards.ToList())
                    {
                        Flashcards.Remove(flashcard);
                    }
                }
                else
                {
                    Debug.WriteLine("BulkDeleteFlashcards: Failed to delete flashcards.");

                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete selected flashcards.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BulkDeleteFlashcards Exception: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Error", $"Error deleting flashcards: {ex.Message}", "OK");
            }
        }

        private async Task EditFlashcard(Flashcard flashcard)
        {
            if (flashcard == null) return;
            await Shell.Current.GoToAsync($"/EditFlashcardPage?flashcardId={flashcard.Id}");
        }

        private async Task NavigateToCreatePage()
        {
            await Shell.Current.Navigation.PushAsync(new CreateFlashcardPage());
        }
    }
}
