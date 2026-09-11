using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace org.Ui.Converters
{
    public class UshortToBrushConverter : BrushConverter<ushort>
    {
        public static UshortToBrushConverter Green = new UshortToBrushConverter()
        {
            Colors = new Dictionary<ushort, Brush>
            {
                {0,Brushes.Green}
            }
        };


        public UshortToBrushConverter()
            : base(new Dictionary<ushort, Brush>
            {
                {0,Brushes.Green }
            },
            Brushes.Red)
        {
        }
    }

    public class Int32ToBrushConverter : BrushConverter<int>
    {
        public static Int32ToBrushConverter Green = new Int32ToBrushConverter()
        {
            Colors = new Dictionary<int, Brush>
            {
                {0,Brushes.Green}
            }
        };


        public Int32ToBrushConverter()
            : base(new Dictionary<int, Brush>
            {
                {0,Brushes.Green }
            },
            Brushes.Red)
        {
        }
    }
}
