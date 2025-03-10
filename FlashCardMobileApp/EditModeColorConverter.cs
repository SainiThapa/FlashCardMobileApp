using System;
using System.Globalization;
using Xamarin.Forms;

namespace FlashCardMobileApp
{
    public class EditModeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEditing = (bool)value;
            return isEditing ? Color.White : Color.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
