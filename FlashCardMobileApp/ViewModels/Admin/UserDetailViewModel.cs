using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class UserDetailViewModel:BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<UserFlashcardViewModel> Flashcards { get; set; }
        public ObservableCollection<UserFlashcardViewModel> SelectedFlashcards { get; set; }
        public ICommand DeleteSelectedFlashcardsCommand { get; }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(); // Notify the UI of the property change
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(); // Notify the UI of the property change
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(); // Notify the UI of the property change
            }
        }


        public string FullName => $"{FirstName} {LastName}"; 

        public UserDetailViewModel()
        {
            _apiService = new ApiService();
            Flashcards = new ObservableCollection<UserFlashcardViewModel>();
            SelectedFlashcards = new ObservableCollection<UserFlashcardViewModel>();

            DeleteSelectedFlashcardsCommand = new Command(DeleteSelectedFlashcards);
        }
        public async Task LoadUserDetails(string userId)
        {
            IsBusy = true;
            try
            {

            var userDetails = await _apiService.GetUserDetailAsync(userId);

            if (userDetails != null)
            {
                FirstName = userDetails.FirstName;
                LastName = userDetails.LastName;
                Email = userDetails.Email;

                // Clear the existing list before adding new items
                Flashcards.Clear();
                foreach (var flashcard in userDetails.Flashcards)
                {
                    Flashcards.Add(flashcard);
                }

                // Notify the UI that data has been loaded successfully
                OnPropertyChanged(nameof(Flashcards));
            }
            }
            catch(Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load user details: {e.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void DeleteSelectedFlashcards()
        {
            if (!SelectedFlashcards.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No Flashcards selected.", "OK");
                return;
            }
            var FlashcardIds = SelectedFlashcards.Select(t => t.Id).ToList();
            Debug.WriteLine($"Flash IDs to delete: {string.Join(", ", FlashcardIds)}");

            bool isDeleted = await _apiService.BulkDeleteFlashcardsAsync(FlashcardIds);

            if (isDeleted)
            {
                foreach (var task in SelectedFlashcards.ToList())
                {
                    Flashcards.Remove(task);
                }
                SelectedFlashcards.Clear();
                await Application.Current.MainPage.DisplayAlert("Success", "Selected Flashcards deleted.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete selected Flashcards.", "OK");
            }
        }

        }
}
