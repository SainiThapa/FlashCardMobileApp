using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using FlashCardMobileApp.Services;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminViewUserViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public string UserId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ObservableCollection<UserFlashcardViewModel> Flashcards { get; set; }

        public AdminViewUserViewModel()
        {
            _apiService = new ApiService();
            Flashcards = new ObservableCollection<UserFlashcardViewModel>();
        }

        public async Task LoadUserDetails(string userId)
        {
            UserId = userId;
            var userDetails = await _apiService.GetUserDetailAsync(userId);

            if (userDetails != null)
            {
                FirstName = userDetails.FirstName;
                LastName = userDetails.LastName;
                Email = userDetails.Email;

                Flashcards.Clear();
                foreach (var flashcard in userDetails.Flashcards)
                    Flashcards.Add(flashcard);
            }
        }
    }
}
