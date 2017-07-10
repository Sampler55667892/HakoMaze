using System.Windows;
using System.Windows.Input;
using FakeFrame;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // DataContext が変更される前に追加
            DataContextChanged += MainWindow_DataContextChanged;

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
                    "2017/06/18～2017/07/10 (休み休み作成)\r\n" +
                    "Written by Kodera Hiroshi.\r\n" +
                    "(マニュアルのルールによる探索のみ (個人の趣味で) 作成予定)\r\n" +
                    "※監視する意味はほとんどありませんよ.";
            }
        }
    }
}
