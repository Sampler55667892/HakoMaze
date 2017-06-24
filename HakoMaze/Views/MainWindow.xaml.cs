using System.Windows;
using System.Windows.Input;
using HakoMaze.Common;
using HakoMaze.ViewModels;

namespace HakoMaze.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // DataContext が変更される前に追加
            DataContextChanged += MainWindow_DataContextChanged;
            Loaded += MainWindow_Loaded;

            InitializeComponent();
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

                // バージョン情報を表示
                vm.HistoryMessageAreaText = "2017/06/18～2017/06/24 Written by Kodera Hiroshi.";
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;

            if (DataContext is MainWindowViewModel vm) {
                // MazeFrameView相対の座標計算用 (Canvas相対に設定するとずれる)
                vm.CommandScheduler.ChildView = this.FindFirst<MazeFrameView>();
                vm.CommandScheduler.ComputesRelativePositionFromChildView = true;
            }
        }
    }
}
