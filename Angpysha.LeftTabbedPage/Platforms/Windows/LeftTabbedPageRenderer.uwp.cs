using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Angpysha.LeftTabbedPage.UWP.Controls;
using Plugin.Angpysha.LeftTabbedPage.Shared;
using Plugin.Angpysha.LeftTabbedPage.UWP;
using Plugin.Angpysha.LeftTabbedPage.UWP.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using ColumnDefinition = Windows.UI.Xaml.Controls.ColumnDefinition;
using ColumnDefinitionCollection = Windows.UI.Xaml.Controls.ColumnDefinitionCollection;
using Grid = Windows.UI.Xaml.Controls.Grid;
using GridLength = Windows.UI.Xaml.GridLength;
using GridUnitType = Windows.UI.Xaml.GridUnitType;
using ListView = Windows.UI.Xaml.Controls.ListView;

[assembly: ExportRenderer(typeof(LeftTabbedPage),typeof(UwpLeftTabbedPageRenderer))]
namespace Plugin.Angpysha.LeftTabbedPage.UWP
{
    public class UwpLeftTabbedPageRenderer : LeftTabbedMainContainer, IVisualElementRenderer
    {

        public UwpLeftTabbedPageRenderer() : base()
        {
          
        }

        public void Dispose()
        {
            
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var constraint = new Windows.Foundation.Size(widthConstraint, heightConstraint);

            double oldWidth = this.Width;
            double oldHeight = this.Height;

            this.Height = double.NaN;
            this.Width = double.NaN;

            this.Measure(constraint);
            var result = new Size(Math.Ceiling(this.DesiredSize.Width), Math.Ceiling(this.DesiredSize.Height));

           this.Width = oldWidth;
           this.Height = oldHeight;

            return new SizeRequest(result);
        }

        public void SetElement(VisualElement element)
        {
            var oldElement = Element;
            Element = element;
            this.Loaded += OnLoaded;
            //var def1 = new ColumnDefinition();
            //def1.Width = new GridLength(64, Gr)
          
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //var pp = new LeftTabbedMainPageContainer
            //{
            //    Width = 500,
            //    Height = 500
            //};
            //this.Children.Add(pp);
            //var ff = new TextBlock();
            //ff.Text = "fafasdaf";
            //this.Children.Add(ff);
            var def = new List<ColumnDefinition>
            {
                new ColumnDefinition()
                {
                    Width = new GridLength(64)
                },
                new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                }
            };
            var grd = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition()
                    {
                        Width = new GridLength(64)
                    },
                    new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    }
                },
                //Background = new Windows.UI.Xaml.Media.SolidColorBrush(Color.Orange.ToWindowsColor())
            };
            var listView = new ListView();
            listView.HorizontalAlignment = HorizontalAlignment.Right;
            listView.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Color.Red.ToWindowsColor());
            SetMenuItems(listView);
            grd.Children.Add(listView);
            Children.Add(grd);

            //ColumnDefinitions = def;
        }

        private void SetMenuItems(ListView listView)
        {
            var tabbed = Element as Shared.LeftTabbedPage;
            var menuitems = tabbed.Children.Select(x => new Shared.MenuItem()
            {
                Title = x.Title,
                IconImageSource = x.IconImageSource
            }).ToList();
            listView.ItemsSource = menuitems;
            
            //  var xfTempale = tabbed.TabItemTemplate.ToNative();
        }

        public UIElement GetNativeElement()
        {
            return this;
        }

        public FrameworkElement ContainerElement => this;
        public VisualElement Element { get; private set; }
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
    }
    //public class UwpLeftTabbedPageRenderer : VisualElementRenderer<Shared.LeftTabbedPage, LeftTabbedMainContainer>
    //{

    //}
    //public class UwpLeftTabbedPageRenderer : IVisualElementRenderer
    //{
    //    public LeftTabbedMainContainer Control { get; set; }
    //    public Shared.LeftTabbedPage LeftTabbedPage { get; set; }
    //    public void Dispose()
    //    {

    //    }

    //    public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
    //    {
    //        return new SizeRequest(new Size(150, 150));
    //    }

    //    public void SetElement(VisualElement element)
    //    {
    //      //  Element = LeftTabbedPage = element;
    //      Element = element;
    //      LeftTabbedPage = (Shared.LeftTabbedPage) element;
    //      Control = new LeftTabbedMainContainer();
    //    }

    //    public UIElement GetNativeElement()
    //    {
    //        return Control;
    //    }

    //    public FrameworkElement ContainerElement { get; private set; }
    //    public VisualElement Element { get; private set; }
    //    public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
    //}
}
