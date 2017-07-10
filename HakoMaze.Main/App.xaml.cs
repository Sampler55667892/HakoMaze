using System.Windows;
using FakeFrame;
using HakoMaze.Data;
using HakoMaze.Main.ViewModels;
using HakoMaze.Main.Views;

namespace HakoMaze.Main
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
            MainWindow.Loaded += MainWindow_Loaded;

            var mainWindowViewModel = new MainWindowViewModel {
                CanvasViewModel = new MazeFrameViewModel( mazeFrameData, mazeContentData ) { Size = 380, Margin = 10 },
                TreeViewModel = new RedboxTraceTreeViewModel { Visibility = Visibility.Hidden }
            };
            mainWindow.DataContext = mainWindowViewModel;

            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }

        protected override void OnExit( ExitEventArgs e )
        {
            base.OnExit( e );

            CommandQueue.Instance.Dispose();
        }

        void MainWindow_Loaded( object sender, RoutedEventArgs e )
        {
            var mainWindow = sender as MainWindow;

            mainWindow.Loaded -= MainWindow_Loaded;

            // MazeFrameView相対の座標計算用 (Canvas相対に設定するとずれる)
            if (mainWindow.DataContext is MainWindowViewModel vm)
                commandScheduler.ChildView = mainWindow.FindFirst<MazeFrameView>();
        }
    }
}
