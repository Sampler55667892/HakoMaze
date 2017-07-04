using System.Windows;
using System.Windows.Input;
using FakeFrame;
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
            if (e.Source is MazeFrameView)
                return;
            DragMove();
        }

        void MainWindow_DataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            DataContextChanged -= MainWindow_DataContextChanged;

            if (DataContext is MainWindowViewModel vm) {
                vm.CloseCommand = new RelayCommand( x => Close() );

                // バージョン情報などを表示
                vm.HistoryMessageAreaText =
                    "2017/06/18～2017/06/25, 2017/7/3～2017/7/4\r\n" +
                    "Written by Kodera Hiroshi.\r\n" +
                    "(マニュアルのルールによる探索のみ (個人の趣味で) 作成予定)\r\n" +
                    "※監視する意味はほとんどありませんよ.";
            }
        }

        void MainWindow_Loaded( object sender, RoutedEventArgs e )
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
