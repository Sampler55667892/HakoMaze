using System;
using HakoMaze.Common;

namespace HakoMaze.ViewModels
{
    public class MainWindowCommand : Command
    {
        protected new MainWindowViewModel ViewModel { get; private set; }
        protected MazeFrameViewModel CanvasViewModel => ViewModel.CanvasViewModel;

        public MainWindowCommand( MainWindowViewModel vm ) : base( vm ) =>
            this.ViewModel = base.ViewModel as MainWindowViewModel;

        public override void OnInitialize()
        {
            base.OnInitialize();

            AddHistoryMessage( $"{GetDateTimeString()}  {GetType().Name} を開始" );
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            AddHistoryMessage( $"{GetDateTimeString()}  {GetType().Name} を終了" );
        }

        protected void AddHistoryMessage( string message ) =>
            ViewModel.HistoryMessageAreaText = message + "\r\n" + ViewModel.HistoryMessageAreaText;

        protected void UpdateRenderCanvas() => CanvasViewModel?.UpdateRenderCommand.Execute( null );

        string GetDateTimeString() => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
