using System;
using System.Windows.Input;
using HakoMaze.Common;

namespace Common.WpfCommands
{
    public class SwitchCommand : ICommand
    {
        Action execute { get; set; }

        #pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 0067

        public bool CanExecute( object parameter ) => true;

        public void Execute( object parameter ) => execute();

        public SwitchCommand( CommandScheduler commandScheduler, Command nextCommand ) =>
            execute = () => commandScheduler.ActiveCommand = nextCommand;
    }
}
