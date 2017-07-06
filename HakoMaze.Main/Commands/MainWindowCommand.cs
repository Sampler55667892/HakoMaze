using System;
using FakeFrame;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class MainWindowCommand : Command
    {
        protected new MainWindowViewModel ViewModel { get; private set; }
        protected MazeFrameViewModel CanvasViewModel => ViewModel.CanvasViewModel;
        protected RedboxTraceTreeViewModel TreeViewModel => ViewModel.TreeViewModel;

        public MainWindowCommand( MainWindowViewModel vm ) : base( vm ) =>
            this.ViewModel = base.ViewModel as MainWindowViewModel;

        public override void OnInitialize()
        {
            base.OnInitialize();

            AddHistoryMessage( $"{GetType().Name} を開始" );
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            AddHistoryMessage( $"{GetType().Name} を終了" );
        }

        protected void AddHistoryMessage( string message ) =>
            ViewModel.HistoryMessageAreaText = $"{GetDateTimeString()}  {message}\r\n{ViewModel.HistoryMessageAreaText}";

        protected void UpdateRenderCanvas() => CanvasViewModel?.UpdateRenderCommand.Execute( null );

        string GetDateTimeString() => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
