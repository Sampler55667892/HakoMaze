using System.Collections.Generic;

namespace FakeFrame
{
    public interface ISender<T>
    {
        void Broadcast( ObjectMessage<T> message );

        ICollection<IListener<T>> Listeners { get; }
    }
}
