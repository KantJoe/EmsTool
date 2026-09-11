using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using org.Product01.Messagings;
using org.Models.Messagings;
using org.Ui.Messagings;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Product01.ViewModels
{
    public partial class EmProduct01ViewModel : ObservableObject,IRecipient<UshortMessage>
    {
        [ObservableProperty]
        private GenericInfoMessage _message;


        public EmProduct01ViewModel()
        {
            Message = new GenericInfoMessage();
        }


        public void Receive(UshortMessage message)
        {
            Message.ReceivedMessage(message);
        }
    }
}
