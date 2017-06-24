using System.Windows;
using System.Windows.Input;
using HakoMaze.Common;
using HakoMaze.ViewModels;

namespace HakoMaze.Views
{
    public partial class MainWindow : Window
    {
        CommandScheduler commandScheduler;

        public MainWindow()
        {
            // DataContext が変更される前に追加
            DataContextChanged += MainWindow_DataContextChanged;
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            commandScheduler = new CommandScheduler( this );
            DataContext = new MainWindowViewModel( commandScheduler );
        }

        protected override void OnMouseLeftButtonDown( MouseButtonEventArgs e )
        {
            base.OnMouseLeftButtonDown( e );
            DragMove();
        }

        void MainWindow_DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            DataContextChanged -= MainWindow_DataContextChanged;

            if (DataContext is MainWindowViewModel vm) {
                vm.CloseCommand = new RelayCommand( x => Close() );

                var canvasVm = new MazeFrameViewModel { Size = 380, Margin = 10 };
                vm.CanvasViewModel = canvasVm;

                // バージョン情報を表示
                vm.HistoryMessageAreaText = "2017/06/18～2017/06/24 Written by Kodera Hiroshi.";
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;

            // MazeFrameView相対の座標計算用 (Canvas相対に設定するとずれる)
            commandScheduler.ChildView = this.FindFirst<MazeFrameView>();
            commandScheduler.ComputesRelativePositionFromChildView = true;
        }
    }
}
