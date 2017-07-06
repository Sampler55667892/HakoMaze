using System.Collections.Generic;

namespace FakeFrame
{
    public abstract class SenderBase<T> : ISender<T>
    {
        List<IListener<T>> listeners = new List<IListener<T>>();

        public ICollection<IListener<T>> Listeners => listeners;

        public void Broadcast( ObjectMessage<T> message ) =>
            listeners.ForEach( x => x.Listen( message ) );

        public void Broadcast( string header, T content ) =>
            Broadcast( new ObjectMessage<T>( header: header, content: content ) );
    }
}
