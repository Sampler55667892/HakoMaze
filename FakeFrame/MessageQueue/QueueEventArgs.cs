using System;

namespace FakeFrame
{
    public class QueueEventArgs : EventArgs
    {
        public dynamic Item { get; private set; }

        public QueueEventArgs( dynamic item )
        {
            this.Item = item;
        }
    }
}
