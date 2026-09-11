using org.Models.Messagings;
using org.Utils;

namespace org.Ui.Messagings
{
    public partial class Input125Message : UshortMessage
    {
        public const string KEY = "4_125";

        public Input125Message()
            : base(new ushort[125])
        {

        }

        public Input125Message(ReadOnlyMemory<ushort> buffer)
            : base(buffer)
        {
        }

        public Input125Message(ReadOnlyMemory<ushort> buffer, string mapping)
            : base(buffer, mapping)
        {
        }
    }
}
