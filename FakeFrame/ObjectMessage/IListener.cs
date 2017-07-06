namespace FakeFrame
{
    public interface IListener<T>
    {
        void Listen( ObjectMessage<T> message );
    }
}
