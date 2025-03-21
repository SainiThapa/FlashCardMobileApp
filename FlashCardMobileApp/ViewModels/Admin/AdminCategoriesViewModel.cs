using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;

namespace FlashCardMobileApp.ViewModels.Admin
{
    public class AdminCategoriesViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public AdminCategoriesViewModel()
        {
            Title = "Manage Categories";
            _apiService = new ApiService();
            Categories = new ObservableCollection<Category>();

            LoadCategoriesCommand = new Command(async () => await LoadCategories());
            AddCategoryCommand = new Command(async () => await AddCategory());
            DeleteCategoryCommand = new Command<Category>(async (category) => await DeleteCategory(category));

            LoadCategoriesCommand.Execute(null);
        }

        private async Task LoadCategories()
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
        }

        private async Task DeleteCategory(Category category)
        {
            if (category == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Delete category {category.Name}?", "Yes", "No");
            if (!confirm) return;

            await _apiService.DeleteCategoryAsync(category.Id);
            Categories.Remove(category);
        }
    }
}
