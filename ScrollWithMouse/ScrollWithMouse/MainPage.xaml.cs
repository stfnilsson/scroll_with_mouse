using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using ScrollWithMouse.Helpers;

namespace ScrollWithMouse
{
    public class TestClass
    {
        public string Text { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
    }

    public sealed partial class MainPage : PageBase
    {
        public ObservableCollection<TestClass> List = new ObservableCollection<TestClass>();

        public MainPage()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            List.Clear();
            for (var i = 0; i < 50; i++)
            {
                List.Add(new TestClass
                {
                    Text = "Text " + i,
                    Text2 = "Text " + i,
                    Text3 = "Text " + i
                });
            }
        }

        private async void ListInstance_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new MessageDialog("Listitem tapped");
            await dialog.ShowAsync();
        }
    }
}