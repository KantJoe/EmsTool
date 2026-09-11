using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace org.Ui.ViewModels
{
    public partial class ProductModelViewModel : ObservableObject
    {
        public ProductModel ProductModel { get; private set; }

        public string ModelName => ProductModel?.ModelName;
        /// <summary>
        /// 电池类型
        /// </summary>
        public string BatteryType => ProductModel?.BatteryType;
        /// <summary>
        /// 最小系统容量 Wh
        /// </summary>
        public decimal? MinSystemEnergy => ProductModel?.MinSystemEnergy;
        /// <summary>
        /// 最大系统容量 Wh
        /// </summary>
        public decimal? MaxSystemEnergy => ProductModel?.MaxSystemEnergy;
        /// <summary>
        /// 模块电压 V
        /// </summary>
        public decimal? ModuleVoltage => ProductModel?.ModuleVoltage;
        /// <summary>
        /// 模块容量 Ah
        /// </summary>
        public decimal? ModuleCapacity => ProductModel?.ModuleCapacity;
        /// <summary>
        /// 长 mm
        /// </summary>
        public decimal? Width => ProductModel?.Width;
        /// <summary>
        /// 宽 mm
        /// </summary>
        public decimal? Deepth => ProductModel?.Deepth;
        /// <summary>
        /// 高 mm
        /// </summary>
        public decimal? Height => ProductModel?.Height;
        /// <summary>
        /// 防尘防水等级
        /// </summary>
        public string IngressProtection => ProductModel?.IngressProtection;

        public string LocalPicturePath => ProductModel?.LocalPicturePath;

        public string RecycleLifetime => ProductModel?.RecycleLifetime;

        public decimal? Soc => ProductModel?.SoC;

        public string Weight => ProductModel?.Weight;

        public ImageSource PictureSource { get; private set; }

        public ProductModelViewModel(ProductModel productModel)
        {
            ProductModel = productModel;
            if (!string.IsNullOrEmpty(LocalPicturePath))
            {
                PictureSource = new BitmapImage(new Uri(LocalPicturePath, UriKind.Relative));
            }
        }
    }
}
