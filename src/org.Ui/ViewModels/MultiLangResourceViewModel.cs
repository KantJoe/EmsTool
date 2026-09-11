using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace org.Ui.ViewModels
{
    public class MultiLangResourceViewModel : ObservableObject
    {
        public MultiLangResource Resource { get; private set; }
        public string Code => Resource?.Code;

        public string Display=> Resource?.Display;

        public string Uri =>Resource?.Uri;


        public MultiLangResourceViewModel(MultiLangResource resource)
        {
            Resource = resource;
        }
    }
}
