using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Attendance.LoadingSpinnerControl
{
    public class ScrollViewerExtensions : DependencyObject
    {
        /// <summary>
        /// MAIN property: this activates the whole fading effect
        /// </summary>
        public static readonly DependencyProperty FadedEdgeThicknessProperty =
            DependencyProperty.RegisterAttached("FadedEdgeThickness", typeof(double), typeof(ScrollViewerExtensions), new PropertyMetadata(20.0d, OnFadedEdgeThicknessChanged));

        public static void SetFadedEdgeThickness(ScrollViewer s, double value)
        {
            s.SetValue(FadedEdgeThicknessProperty, value);
        }

        public static double GetFadedEdgeThickness(ScrollViewer s, double value)
        {
            return (double)s.GetValue(FadedEdgeThicknessProperty);
        }

        /// <summary>
        /// optional property. changes how fast the fade appears/diappears when scrolling near an edge
        /// </summary>
        public static readonly DependencyProperty FadedEdgeFalloffSpeedProperty =
            DependencyProperty.RegisterAttached("FadedEdgeFalloffSpeed", typeof(double), typeof(ScrollViewerExtensions), new PropertyMetadata(4.0d, OnFadedEdgeFalloffSpeedChanged));

        public static void SetFadedEdgeFalloffSpeed(ScrollViewer s, double value)
        {
            s.SetValue(FadedEdgeFalloffSpeedProperty, value);
        }

        public static double GetFadedEdgeFalloffSpeed(ScrollViewer s, double value)
        {
            return (double)s.GetValue(FadedEdgeFalloffSpeedProperty);
        }

        /// <summary>
        /// optional property. changes how opaque the outermost edge should be
        /// </summary>
        public static readonly DependencyProperty FadedEdgeOpacityProperty =
            DependencyProperty.RegisterAttached("FadedEdgeOpacity", typeof(double), typeof(ScrollViewerExtensions), new PropertyMetadata(0.0d, OnFadedEdgeOpacityChanged));

        public static void SetFadedEdgeOpacity(ScrollViewer s, double value)
        {
            s.SetValue(FadedEdgeOpacityProperty, value);
        }

        public static double GetFadedEdgeOpacity(ScrollViewer s, double value)
        {
            return (double)s.GetValue(FadedEdgeOpacityProperty);
        }




        private const string PART_SCROLL_PRESENTER_CONTAINER_NAME = "PART_ScrollContentPresenterContainer";

        private static Dictionary<ScrollViewer, FadeSettings> Settings = new Dictionary<ScrollViewer, FadeSettings>();



        /// <summary>
        /// this is kindof the constructor for the properties. If you don't specify this, nothing will fade!
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void OnFadedEdgeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer == null)
                return;

            double edgeThickness = (double)e.NewValue;

            scrollViewer.ScrollChanged += FadingScrollViewer_ScrollChanged;
            scrollViewer.SizeChanged += FadingScrollViewer_SizeChanged;

            if (!Settings.ContainsKey(scrollViewer))
                Settings.Add(scrollViewer, new FadeSettings());

            Settings[scrollViewer].FadedEdgeThickness = edgeThickness;
        }


        public static void OnFadedEdgeFalloffSpeedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer == null)
                return;

            double edgeFalloffSpeed = (double)e.NewValue;

            if (!Settings.ContainsKey(scrollViewer))
                Settings.Add(scrollViewer, new FadeSettings());

            Settings[scrollViewer].FadedEdgeFalloffSpeed = edgeFalloffSpeed;
        }


        public static void OnFadedEdgeOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer == null)
                return;

            double edgeOpacity = (double)e.NewValue;

            if (!Settings.ContainsKey(scrollViewer))
                Settings.Add(scrollViewer, new FadeSettings());

            Settings[scrollViewer].FadedEdgeOpacity = edgeOpacity;
        }


        private static void FadingScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            FadeSettings settings = Settings[scrollViewer];

            if (settings.InnerFadedBorder == null)
                return;

            var topOffset = CalculateNewMarginBasedOnOffsetFromEdge(scrollViewer, scrollViewer.VerticalOffset); ;
            var bottomOffset = CalculateNewMarginBasedOnOffsetFromEdge(scrollViewer, scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset);
            var leftOffset = CalculateNewMarginBasedOnOffsetFromEdge(scrollViewer, scrollViewer.HorizontalOffset);
            var rightOffset = CalculateNewMarginBasedOnOffsetFromEdge(scrollViewer, scrollViewer.ScrollableWidth - scrollViewer.HorizontalOffset);

            settings.InnerFadedBorder.Margin = new Thickness(leftOffset, topOffset, rightOffset, bottomOffset);
        }

        private static void FadingScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            FadeSettings settings = Settings[scrollViewer];

            if (!settings.Initialized) // abuse the SizeChanged event to call the OnApplyTemplate method. We can't override it, so we need something, that fires after it would normally be called. see http://msdn.microsoft.com/en-us/library/dd351483%28v=vs.95%29.aspx
            {
                OnApplyTemplate(scrollViewer);
                settings.Initialized = true;
            }

            if (settings.OuterFadedBorder == null || settings.InnerFadedBorder == null)
                return;

            settings.OuterFadedBorder.Width = e.NewSize.Width;
            settings.OuterFadedBorder.Height = e.NewSize.Height;

            double innerFadedBorderBaseMarginThickness = settings.FadedEdgeThickness / 2.0;
            settings.InnerFadedBorder.Margin = new Thickness(innerFadedBorderBaseMarginThickness);
           // settings.InnerFadedBorderEffect.Radius = settings.FadedEdgeThickness;
        }

        private static double CalculateNewMarginBasedOnOffsetFromEdge(ScrollViewer scrollViewer, double edgeOffset)
        {
            FadeSettings settings = Settings[scrollViewer];

            var innerFadedBorderBaseMarginThickness = settings.FadedEdgeThickness / 2.0;
            //var calculatedOffset = (innerFadedBorderBaseMarginThickness) - (1.0 * (this.FadedEdgeThickness - (edgeOffset / this.FadedEdgeFalloffSpeed)));

            double calculatedOffset;
            if (edgeOffset == 0)
                calculatedOffset = -innerFadedBorderBaseMarginThickness;
            else
                calculatedOffset = (edgeOffset * settings.FadedEdgeFalloffSpeed) - innerFadedBorderBaseMarginThickness;

            return Math.Min(innerFadedBorderBaseMarginThickness, calculatedOffset);
        }

        public static void OnApplyTemplate(ScrollViewer scrollViewer)
        {
            //BuildInnerFadedBorderEffectForOpacityMask(scrollViewer);
            BuildInnerFadedBorderForOpacityMask(scrollViewer);
            BuildOuterFadedBorderForOpacityMask(scrollViewer);
            SetOpacityMaskOfScrollContainer(scrollViewer);
        }

        private static void BuildInnerFadedBorderEffectForOpacityMask(ScrollViewer scrollViewer)
        {
            FadeSettings settings = Settings[scrollViewer];

           /* settings.InnerFadedBorderEffect = new BlurEffect()
            {
                RenderingBias = RenderingBias.Performance,
            };*/
        }

        private static void BuildInnerFadedBorderForOpacityMask(ScrollViewer scrollViewer)
        {
            FadeSettings settings = Settings[scrollViewer];

            settings.InnerFadedBorder = new Border()
            {
                Background = Brushes.Black,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                //Effect = settings.InnerFadedBorderEffect,
            };
        }

        private static void BuildOuterFadedBorderForOpacityMask(ScrollViewer scrollViewer)
        {
            FadeSettings settings = Settings[scrollViewer];

            byte fadedEdgeByteOpacity = (byte)(settings.FadedEdgeOpacity * 255);

            settings.OuterFadedBorder = new Border()
            {
                Background = new SolidColorBrush(Color.FromArgb(fadedEdgeByteOpacity, 0, 0, 0)),
                ClipToBounds = true,
                Child = settings.InnerFadedBorder,
            };
        }

        private static void SetOpacityMaskOfScrollContainer(ScrollViewer scrollViewer)
        {
            FadeSettings settings = Settings[scrollViewer];

            var opacityMaskBrush = new VisualBrush()
            {
                Visual = settings.OuterFadedBorder
            };

            var scrollContentPresentationContainer = scrollViewer.Template.FindName(PART_SCROLL_PRESENTER_CONTAINER_NAME, scrollViewer) as UIElement;

            if (scrollContentPresentationContainer == null)
                return;

            scrollContentPresentationContainer.OpacityMask = opacityMaskBrush;

            // test
            /*var container = scrollContentPresentationContainer as Border;
            var scroller = container.Child as UIElement;
            container.Child = null;

            Grid g = new Grid();
            container.Child = g;

            g.Children.Add(scroller);
            this.OuterFadedBorder.IsHitTestVisible = false;
            g.Children.Add(this.OuterFadedBorder);*/
        }


        protected class FadeSettings
        {           
            public Border InnerFadedBorder { get; set; }
            public Border OuterFadedBorder { get; set; }

            public double FadedEdgeThickness { get; set; }
            public double FadedEdgeFalloffSpeed { get; set; }
            public double FadedEdgeOpacity { get; set; }

            public bool Initialized { get; set; }

            public FadeSettings()
            {
                FadedEdgeThickness = 20.0d;
                FadedEdgeFalloffSpeed = 4.0d;
                FadedEdgeOpacity = 0.0d;
            }
        }
    }
}
