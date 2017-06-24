using System.Windows;
using HakoMaze.Models;
using HakoMaze.ViewModels;
using HakoMaze.Views;
using HakoMaze.Common;

namespace HakoMaze
{
    public partial class App : Application
    {
        MazeFrameData mazeFrameData = new MazeFrameData();
        MazeContentData mazeContentData = new MazeContentData();
        CommandScheduler commandScheduler;

        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );

            var mainWindow = new MainWindow();
            commandScheduler = new CommandScheduler( mainWindow );
            var mainWindowViewModel = new MainWindowViewModel( commandScheduler );
            mainWindow.DataContext = mainWindowViewModel;
            var canvasViewModel = new MazeFrameViewModel( mazeFrameData, mazeContentData ) { Size = 380, Margin = 10 };
            mainWindowViewModel.CanvasViewModel = canvasViewModel;

            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }
    }
}
