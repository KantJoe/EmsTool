using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// HomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class HomeControl : UserControl
    {
        private static BitmapImage _homeBackground = new BitmapImage(new Uri("Resources/Images/home.jpg", UriKind.RelativeOrAbsolute));
        public HomeControl()
        {
            InitializeComponent();
            _homeBackground.Freeze();
            this.Background = new ImageBrush()
            {
                ImageSource = _homeBackground
            };

            this.DataContext = new HomeViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _text = "hahahahah";
    }
}
