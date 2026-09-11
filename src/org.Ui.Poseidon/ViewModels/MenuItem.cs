using CommunityToolkit.Mvvm.ComponentModel;
using org.Ui.MultiLanguage;
using org.Ui.Poseidon.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class MenuItem : ObservableObject
    {
        [ObservableProperty]
        private int _orderNumber;

        //[ObservableProperty]
        //private string _code;
        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
                Display = MultiLang.GetString(_code);
            }
        }

        [ObservableProperty]
        private string _display;

        [ObservableProperty]
        private Type _contentType;

        [ObservableProperty]
        private Visibility _visible = Visibility.Visible;

        [ObservableProperty]
        private PackIconKind _selectedIcon;

        [ObservableProperty]
        private PackIconKind _unselectedIcon;

        private object _content;

        public bool Reserved { get; set; }

        /// <summary>
        /// 默认宽：1280-2*16，默认高：900-30-80-2*16
        /// </summary>
        public object Content => _content ?? CreateContent();

        public MenuItem()
        {
            MultiLang.OnLanguageChanged += LanguageChanged;
        }

        ~MenuItem()
        {
            MultiLang.OnLanguageChanged -= LanguageChanged;
        }

        private object CreateContent()
        {
            if (ContentType is null)
            {
                return Activator.CreateInstance(typeof(HomeControl));
            }

            if (Reserved)
            {
                _content = Activator.CreateInstance(ContentType);

                return _content;
            }

            return Activator.CreateInstance(ContentType);
        }

        private void LanguageChanged(string curr, string next)
        {
            Display = MultiLang.GetString(Code);
        }

        public static MenuItem Default { get; }
            = new MenuItem
            {
                ContentType = typeof(HomeControl)
            };

        public void CollectContent()
        {
            var uc = _content as UserControl;
            uc.ReleaseStylusCapture();
            uc.ReleaseMouseCapture();
            uc.ReleaseAllTouchCaptures();
            uc.DataContext = default;
            GC.SuppressFinalize(uc);
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete(500);
        }
    }
}
