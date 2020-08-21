using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Angpysha.LeftTabbedPage.iOS;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCellEx),typeof(ViewCellExRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.iOS
{
    public class ViewCellExRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}
