using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace org.Ui.Converters
{
    public class BooleanToBrushConverter : BrushConverter<bool>
    {
        public static BooleanToBrushConverter White = new BooleanToBrushConverter()
        {
            Colors = new Dictionary<bool, Brush>
            {
                {true,Brushes.White }
            }
        };

        public static BooleanToBrushConverter LightGreen = new BooleanToBrushConverter()
        {
            Colors = new Dictionary<bool, Brush>
            {
                {true,Brushes.LightGreen}
            }
        };


        public BooleanToBrushConverter()
            : base(new Dictionary<bool, Brush>
            {
                {true,Brushes.White }
            },
            Brushes.DimGray)
        {
        }
    }
}
