using System.Collections.Generic;

namespace Properly.Web.ViewModels.Messages
{
    public class MessagesViewModel
    {
        public IEnumerable<MessageViewModel> ActiveMessages { get; set; }
        public IEnumerable<MessageViewModel> ArchivedMessages { get; set; }
    }
}
