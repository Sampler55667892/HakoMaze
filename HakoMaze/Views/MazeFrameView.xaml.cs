using System.Windows;
using System.Windows.Controls;
using HakoMaze.Common;
using HakoMaze.ViewModels;

namespace HakoMaze.Views
{
    public partial class MazeFrameView : UserControl
    {
        public MazeFrameView()
        {
            Loaded += MazeFrameView_Loaded;
            InitializeComponent();
        }

        // DataContextChanged のタイミングだと DataContext はまだ変わっていない
        private void MazeFrameView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MazeFrameView_Loaded;
            SetCommand();
        }

        void SetCommand()
        {
            if (DataContext is MazeFrameViewModel vm) {
                vm.AddViewElementCommand = new RelayCommand( x => {
                    if (x is UIElement ui) {
                        var canvas = this.FindFirst<Canvas>();
                        if (canvas != null)
                            canvas.Children.Add( ui );
                    }
                });

                vm.ClearViewElementCommand = new RelayCommand( x => {
                    var canvas = this.FindFirst<Canvas>();
                    if (canvas != null)
                        canvas.Children.Clear();
                    // ここでは壁データはクリアしない
                });
            }
        }
    }
}
