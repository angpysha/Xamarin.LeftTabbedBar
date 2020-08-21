using System;

using Angpysha.LeftTabbedPage.Demo.Models;

namespace Angpysha.LeftTabbedPage.Demo.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
