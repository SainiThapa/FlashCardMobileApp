using System;
using System.Collections.Generic;
using System.Text;

namespace FlashCardMobileApp.Models
{
    public class UserProfile
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TaskCount { get; set; }
    }
}
