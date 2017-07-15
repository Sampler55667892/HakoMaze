using System.Windows;
using System.Windows.Controls;
using FakeFrame;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Views
{
    public partial class MazeFrameView : UserControl
    {
        public MazeFrameView()
        {
            Loaded += MazeFrameView_Loaded;
            InitializeComponent();
        }

        // DataContextChanged のタイミングだと DataContext はまだ変わっていない
        void MazeFrameView_Loaded( object sender, RoutedEventArgs e )
        {
            Loaded -= MazeFrameView_Loaded;
            SetCommand();
        }

        void SetCommand()
        {
            if (!(DataContext is MazeFrameViewModel vm))
                return;

            vm.AddViewItemCommand = new RelayCommand( x => {
                if (!(x is FrameworkElement ui))
                    return;
                var canvas = this.FindFirst<Canvas>();
                if (canvas != null) {
                    if (ui.Tag is Point position) {
                        Canvas.SetLeft( ui, position.X );
                        Canvas.SetTop( ui, position.Y );
                    }
                    canvas.Children.Add( ui );
                }
            });

            vm.ClearViewItemsCommand = new RelayCommand( x => {
                var canvas = this.FindFirst<Canvas>();
                if (canvas != null)
                    canvas.Children.Clear();
                // ここでは壁データはクリアしない (View要素のみクリア)
            });
        }
    }
}
