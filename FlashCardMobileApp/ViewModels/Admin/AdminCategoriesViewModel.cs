using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System.Diagnostics;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminCategoriesViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddCategoryCommand { get; }

        public AdminCategoriesViewModel()
        {
            Title = "Manage Categories";
            _apiService = new ApiService();
            Categories = new ObservableCollection<Category>();

            LoadCategoriesCommand = new Command(async () => await LoadCategories());
            AddCategoryCommand = new Command(async () => await AddCategory());

            LoadCategoriesCommand.Execute(null);
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
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddCategory()
        {
            string categoryName = await Application.Current.MainPage.DisplayPromptAsync("New Category", "Enter category name:");
            if (!string.IsNullOrEmpty(categoryName))
            {
                await _apiService.AddCategoryAsync(new Category { Name = categoryName });
                await LoadCategories();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Category error", "Category name cannot be empty", "OK");
            }
        }
    }
}
