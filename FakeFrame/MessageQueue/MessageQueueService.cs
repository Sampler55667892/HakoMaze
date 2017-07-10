using System;
using System.Collections.Generic;

namespace FakeFrame
{
    public class MessageQueueService
    {
        Dictionary<Type, Queue<dynamic>> slots = new Dictionary<Type, Queue<dynamic>>();

        public event QueueEventHandler EnqueueEvent;
        public event QueueEventHandler DequeueEvent;

        public bool AddSlot<T>()
        {
            var type = typeof(T);
            if (slots.ContainsKey( type ))
                return false;
            slots.Add( type, new Queue<dynamic>() );
            return true;
        }

        public bool RemoveSlot<T>()
        {
            var type = typeof(T);
            if (!slots.ContainsKey( type ))
                return false;
            slots[ type ].Clear();
            slots[ type ] = null;
            slots.Remove( type );
            return true;
        }

        public void Enqueue<T>( T item )
        {
            var type = typeof(T);
            if (!slots.ContainsKey( type ))
                throw new Exception( "!slots.ContainsKey( type )" );

            slots[ type ].Enqueue( item );
            EnqueueEvent?.Invoke( type, new QueueEventArgs( item ) );
        }

        public T Dequeue<T>()
        {
            var type = typeof(T);
            if (!slots.ContainsKey( type ))
                throw new Exception( "!slots.ContainsKey( type )" );

            var item = slots[ type ].Dequeue();
            DequeueEvent?.Invoke( type, new QueueEventArgs( item ) );
            return item;
        }
    }
}
