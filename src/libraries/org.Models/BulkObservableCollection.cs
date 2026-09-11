using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace org.Models
{
    public class BulkObservableCollection<T> : ObservableCollection<T>
    {
        private bool _notNotification;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_notNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items?.Any() != true)
            {
                return;
            }

            _notNotification = true;

            foreach (var item in items)
            {
                Items.Add(item);
            }

            _notNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Reset(IEnumerable<T> items)
        {
            _notNotification = true;
            Items.Clear();

            if (items?.Any() == true)
            {
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }

            _notNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
