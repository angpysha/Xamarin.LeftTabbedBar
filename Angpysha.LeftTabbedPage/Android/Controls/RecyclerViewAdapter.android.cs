using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Android.Content;
using Android.Util;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MenuItem = Plugin.Angpysha.LeftTabbedPage.Shared.MenuItem;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class RecyclerViewAdapter : RecyclerView.Adapter
    {

        public IEnumerable<MenuItem> MenuItems { get; set; }
        private int position = 0;
        public WeakReference<Shared.LeftTabbedPage> weakTabbed;

        public Context Context;

        public RecyclerViewAdapter(IEnumerable<MenuItem> menuItems, WeakReference<Shared.LeftTabbedPage> tabbed, Context context)
        {
            MenuItems = menuItems;
            weakTabbed = tabbed;
            Context = context;
        }

        public override int ItemCount => MenuItems?.Count() ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var view = holder.ItemView;

            view.Click += (obj, e) =>
            {
                OnClicked(position);
            };
        }

        private void OnClicked(int position)
        {
            
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (weakTabbed.TryGetTarget(out var tabbed))
            {
                var viewCell = tabbed.TabItemTemplate.CreateContent() as ViewCellEx;

                viewCell.BindingContext = MenuItems.ElementAtOrDefault(position);
                viewCell.Parent = tabbed;

                var sizePx = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 64,
                   Context.Resources.DisplayMetrics);

                var renderer = Platform.CreateRendererWithContext(viewCell.View, Context);
                var aView = renderer.View;
                renderer.Tracker.UpdateLayout();
                var layoutParams = new ViewGroup.LayoutParams(sizePx, sizePx);
                aView.LayoutParameters = layoutParams;
                viewCell.View.Layout(new Rectangle(0, 0, 64, 64));
                position++;
                return new RecyclerViewMenuItemViewHolder(aView);
            }

           return null;
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {

        }
    }

}
