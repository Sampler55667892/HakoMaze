using System.Windows;
using System.Windows.Input;

namespace FakeFrame
{
    public abstract class Command
    {
        protected ViewModelBase ViewModel { get; private set; }
        protected int Step { get; private set; }
        public Point Position { get; internal set; }
        public Key Key { get; internal set; }
        public ModifierKeys ModifierKeys { get; internal set; }
        internal protected bool StopsAct { get; set; }
        internal protected bool Exits { get; set; }
        public dynamic Output { get; private set; }

        public Command( ViewModelBase vm )
        {
            this.ViewModel = vm;
        }

        public virtual void OnInitialize() { }

        public virtual void OnFinalize() { }

        public virtual void OnAct() { }

        public virtual void OnMove() { }

        public virtual void OnKey() { }
    }
}
