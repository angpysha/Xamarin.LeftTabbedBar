using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Provider;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using MenuItem = Plugin.Angpysha.LeftTabbedPage.Shared.MenuItem;
using Microsoft.Maui.Controls;

namespace Plugin.Angpysha.LeftTabbedPage.Android.Controls
{
    public class RecyclerViewMenuItemViewHolder : RecyclerView.ViewHolder
    {
        public DataTemplate DataTemplate { get; set; }
        public MenuItem MenuItem { get; set; }



        public RecyclerViewMenuItemViewHolder(global::Android.Views.View view) : base(view)
        {
           

        }

        
    }
}
