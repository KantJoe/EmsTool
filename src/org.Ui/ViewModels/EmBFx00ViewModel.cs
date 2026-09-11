using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Ui.ViewModels
{
    public partial class EmBFx00ViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProductModelViewModel _productModel;


        public EmBFx00ViewModel(ProductModelViewModel productModel)
        {
            ProductModel = productModel;
        }
    }
}
