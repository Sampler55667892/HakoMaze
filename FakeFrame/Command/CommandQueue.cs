namespace FakeFrame
{
    // {Singleton}
    public class CommandQueue
    {
        static MessageQueueService queueService = new MessageQueueService();
        static CommandQueue instance;

        public event QueueEventHandler EnqueueEvent;
        public event QueueEventHandler DequeueEvent;

        public static CommandQueue Instance
        {
            get {
                if (instance == null)
                    instance = new CommandQueue();
                return instance;
            }
        }

        public CommandQueue() => queueService.AddSlot<Command>();

        public void Dispose() => queueService.RemoveSlot<Command>();

        public void Enqueue( Command command )
        {
            queueService.Enqueue( command );
            EnqueueEvent?.Invoke( typeof(Command), new QueueEventArgs( command ) );
        }

        public Command Dequeue()
        {
            var command = queueService.Dequeue<Command>();
            DequeueEvent?.Invoke( typeof(Command), new QueueEventArgs( command ) );
            return command;
        }
    }
}
