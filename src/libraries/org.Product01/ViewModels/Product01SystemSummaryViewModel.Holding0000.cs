using CommunityToolkit.Mvvm.ComponentModel;
using org.Models.Messagings;
using org.Ui.MultiLanguage;
using org.Utils;

namespace org.Product01.ViewModels
{
    public partial class Product01SystemSummaryViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DTC))]
        public partial ReadOnlyMemory<ushort> Buff_Holding0000 { get; set; }

        public override ushort DTC => Buff_Holding0000.GetPoint(0);

        protected override void Holding0000Refreshing(UshortMessage message)
        {
            base.Holding0000Refreshing(message);

            Buff_Holding0000 = message.Buffer;
        }


    }
}
