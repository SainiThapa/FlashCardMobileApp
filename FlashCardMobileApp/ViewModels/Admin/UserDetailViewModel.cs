using FlashCardMobileApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public async void LoadUserDetails(string userId)
        {
            var userDetails = await _apiService.GetUserDetailAsync(userId);
            Debug.WriteLine($"User detail is loaded as: {JsonConvert.SerializeObject(userDetails, Formatting.Indented)}");
            if (userDetails != null)
            {
                FirstName = userDetails.FirstName;
                LastName = userDetails.LastName;
                Email = userDetails.Email;

                // Map API Flashcards to ViewModel
                foreach (var flashcard in userDetails.Flashcards)
                {
                    Flashcards.Add(new UserFlashcardViewModel
                    {
                        Id = flashcard.Id,
                        Question = flashcard.Question,
                        Answer = flashcard.Answer,
                        CategoryName = flashcard.CategoryName
                    });
                }
            Debug.WriteLine($"FullName: {userDetails.FullName}");
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
