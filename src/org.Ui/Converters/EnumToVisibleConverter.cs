using org.Models;
using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace org.Ui.Converters
{
    public class EnumToVisibleConverter : VisibleConverter<ModbusFunctionCode>
    {
        public static EnumToVisibleConverter HoldingRegister = new EnumToVisibleConverter
        {
            TrueValue = ModbusFunctionCode.ReadHoldingRegisters,
            FalseValue = ModbusFunctionCode.ReadInputRegisters
        };

        public static EnumToVisibleConverter InputRegister = new EnumToVisibleConverter
        {
            TrueValue = ModbusFunctionCode.ReadInputRegisters,
            FalseValue = ModbusFunctionCode.ReadHoldingRegisters
        };

        public EnumToVisibleConverter()
            : base(ModbusFunctionCode.ReadHoldingRegisters, ModbusFunctionCode.ReadInputRegisters)
        {
        }
    }

}
