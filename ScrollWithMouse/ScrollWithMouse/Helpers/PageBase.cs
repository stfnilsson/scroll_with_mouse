using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ScrollWithMouse.Helpers
{
    public abstract class PageBase : Page
    {
        private ScrollViewer _mainScrollViewer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AttachManipulationEvents();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DetachManipulationEvents();
            base.OnNavigatedFrom(e);
        }

        private void AttachManipulationEvents()
        {
            var firstScrollViewer = FindFirstChild<ScrollViewer>(this);
            if (firstScrollViewer == null)
            {
                return;
            }
            _mainScrollViewer = firstScrollViewer;
            _mainScrollViewer.ManipulationMode = ManipulationModes.TranslateY;
            _mainScrollViewer.ManipulationDelta -= ScrollViewer_ManipulationDelta;
            _mainScrollViewer.ManipulationDelta += ScrollViewer_ManipulationDelta;

            _mainScrollViewer.ManipulationCompleted -= ScrollViewer_ManipulationCompleted;
            _mainScrollViewer.ManipulationCompleted += ScrollViewer_ManipulationCompleted;
        }

        private void DetachManipulationEvents()
        {
            if (_mainScrollViewer == null)
            {
                return;
            }
            _mainScrollViewer.ManipulationDelta -= ScrollViewer_ManipulationDelta;
            _mainScrollViewer.ManipulationCompleted -= ScrollViewer_ManipulationCompleted;
        }

        private void ScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //Maybe add scroll here..
            OnManipulationCompleted(e);
        }

        private void ScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_mainScrollViewer == null)
            {
                return;
            }
            var verticalOffset = _mainScrollViewer.VerticalOffset;
            var movement = e.Cumulative.Translation.Y; // *e.Velocities.Linear.Y;
            var newVerticalOffset = verticalOffset - movement;

/*
            Debug.WriteLine(string.Empty);
            Debug.WriteLine(movement);
            Debug.WriteLine("Vertical offset:" + verticalOffset);
            Debug.WriteLine("Y translation:" + e.Delta.Translation.Y);
            Debug.WriteLine("Velocity:" + e.Velocities.Linear.Y);
            Debug.WriteLine("Movement:" + movement);
            Debug.WriteLine("New vertical offset:" + newVerticalOffset);
*/
            _mainScrollViewer.ChangeView(null, newVerticalOffset, null, false);

            OnManipulationDelta(e);
        }

        private static T FindFirstChild<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}