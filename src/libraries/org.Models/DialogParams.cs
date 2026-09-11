using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace org.Models
{
    public class DialogParams
    {
        public DialogAction Action { get; set; }

        public DialogReulst DialogReulst { get; set; }

        public object OriginEntity { get; set; }

        public object UpdatedEntity { get; set; }

    }

    public enum DialogReulst
    {
        NoChanged,
        Failed,
        Succeed
    }

    public enum DialogAction
    {
        None,
        Add,
        Update,
        Remove
    }
}
