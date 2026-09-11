using org.Ui.Poseidon.ViewModels;
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
using System.Windows.Shapes;

namespace org.Ui.Poseidon.Views
{
    /// <summary>
    /// CreateProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        public CreateProjectViewModel Vm;
        public CreateProjectWindow()
        {
            InitializeComponent();

            //PicMain.Source = BitmapFrame.Create(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Resources\\Images\\favicon.ico"), UriKind.Relative));
            this.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Close,
                (t, e) => { this.DialogResult = false; },
                (s, e) => { e.CanExecute = true; }));

            Vm = new CreateProjectViewModel();
            DataContext = Vm;
        }
    }
}
