using System;
using System.Collections.Generic;
using System.Text;

namespace FlashCardMobileApp.Models
{
    public class FlashcardApiModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string UserId { get; set; }
    }
}
