using System;
using System.Windows.Input;

namespace HakoMaze.Common
{
    public class RelayCommand : ICommand
    {
        #pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 0067

        Func<object, bool> canExecuteFunc;
        Action<object> executeAction;

        public RelayCommand( Action<object> executeAction ) : this( x => true, executeAction )
        {
        }

        public RelayCommand( Func<object, bool> canExecuteAction, Action<object> executeAction )
        {
            this.canExecuteFunc = canExecuteAction;
            this.executeAction = executeAction;
        }

        public bool CanExecute( object parameter ) => canExecuteFunc( parameter );

        public void Execute( object parameter ) => executeAction( parameter );
    }
}
