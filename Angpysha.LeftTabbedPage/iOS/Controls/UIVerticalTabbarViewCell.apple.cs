using System;

using Foundation;
using UIKit;

namespace Plugin.Angpysha.LeftTabbedPage
{
    public partial class UIVerticalTabbarViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("UIVerticalTabbarViewCell");
        public static readonly UINib Nib;

        public UIImageView IconView => OutletTabCellIcon;
        public UILabel TitleView => OutletTabCellTitle;

        static UIVerticalTabbarViewCell()
        {
          //  Nib = UINib.FromName("UIVerticalTabbarViewCell", NSBundle.MainBundle);
        }

        protected UIVerticalTabbarViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        
    }
}
