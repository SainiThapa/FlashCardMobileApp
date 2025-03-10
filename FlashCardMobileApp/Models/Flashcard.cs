using System.ComponentModel;

namespace FlashCardMobileApp.Models
{
    public class Flashcard : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }


        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
