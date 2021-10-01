using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;

namespace Plugin.Angpysha.LeftTabbedPage.Shared
{
    public class LeftTabbedPage : TabbedPage
    {
        public static readonly BindableProperty TabItemTemplateProperty = BindableProperty.Create(nameof(TabItemTemplate),
            typeof(DataTemplate),
            typeof(LeftTabbedPage),
            null);

        public DataTemplate TabItemTemplate
        {
            get => (DataTemplate)GetValue(TabItemTemplateProperty);
            set => SetValue(TabItemTemplateProperty, value);
        }

        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header),
            typeof(View),
            typeof(LeftTabbedPage),
            null);

        public View Header
        {
            get => (View)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly BindableProperty FooterProperty = BindableProperty.Create(nameof(Footer),
            typeof(View),
            typeof(LeftTabbedPage),
            null);

        public View Footer
        {
            get => (View)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }

        public static readonly BindableProperty ActiveTabTintColorProperty = BindableProperty.Create(nameof(ActiveTabTintColor),
            typeof(Color),
            typeof(LeftTabbedPage),
            Colors.Blue);

        public Color ActiveTabTintColor
        {
            get => (Color)GetValue(ActiveTabTintColorProperty);
            set => SetValue(ActiveTabTintColorProperty, value);
        }

        public static readonly BindableProperty InactiveTabTintColorProperty = BindableProperty.Create(nameof(InactiveTabTintColor),
            typeof(Color),
            typeof(LeftTabbedPage),
            Colors.Gray);

        public Color InactiveTabTintColor
        {
            get => (Color)GetValue(InactiveTabTintColorProperty);
            set => SetValue(InactiveTabTintColorProperty, value);
        }

        public static BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight),
            typeof(float),
            typeof(LeftTabbedPage),
            120f);

        public float HeaderHeight
        {
            get => (float) GetValue(HeaderHeightProperty);
            set => SetValue(HeaderHeightProperty, value);
        }

        
        public LeftTabbedPage()
        {

        }
    }
}
