using org.Models;
using org.Ui.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace org.Ui.Selectors
{
    public class MultipleDateTypeSelector : DataTemplateSelector
    {
        public DataTemplate TcpTemplate { get; set; }

        public DataTemplate RtuTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var vm = item as EndPointViewModel;

            switch (vm?.CommunicateType)
            {
                case CommunicateType.ModbusRtu:
                    return RtuTemplate;
                case CommunicateType.ModbusTcp:
                    return TcpTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}
