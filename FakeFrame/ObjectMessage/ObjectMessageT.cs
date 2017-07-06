namespace FakeFrame
{
    public class ObjectMessage<T>
    {
        public string Header { get; set; }

        public T Content { get; set; }

        public ObjectMessage( string header, T content )
        {
            this.Header = header;
            this.Content = content;
        }
    }
}
