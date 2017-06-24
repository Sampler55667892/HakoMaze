using System.Windows;

namespace HakoMaze.Common
{
    public abstract class Command
    {
        protected ViewModelBase ViewModel { get; private set; }
        protected int Step { get; private set; }
        public Point Position { get; internal set; }
        public dynamic Output { get; private set; }

        public Command( ViewModelBase vm )
        {
            this.ViewModel = vm;
        }

        public virtual void OnInitialize() { }

        public virtual void OnFinalize() { }

        public virtual void OnAct() { }

        public virtual void OnMove() { }
    }
}
