using System.Windows.Media;
using System.Windows.Shapes;
using HakoMaze.ViewModels;

namespace HakoMaze.Logics
{
    public class DrawMazeFrameLogic
    {
        public void Draw( MainWindowViewModel vm )
        {
            var canvasVm = vm.CanvasViewModel;

            if (canvasVm.MazeFrameData.SizeX == 0 || canvasVm.MazeFrameData.SizeY == 0)
                return;

            // 線分をクリア
            canvasVm.ClearViewElementCommand?.Execute( null );

            DrawFrame( vm, canvasVm );
            DrawWalls( vm, canvasVm );
            DrawBoxes( vm, canvasVm );
        }

        void DrawFrame( MainWindowViewModel vm, MazeFrameViewModel canvasVm )
        {
            var factor = (canvasVm.Size - canvasVm.Margin * 2) / (double)canvasVm.MazeFrameData.SizeX;

            // 縦線
            for (var i = 0; i <= canvasVm.MazeFrameData.SizeX; ++i) {
                var x = canvasVm.Margin + i * factor;
                var yLine = NewLine( x, 0d + canvasVm.Margin, x, canvasVm.Size - canvasVm.Margin, 1d );
                canvasVm.AddViewElementCommand?.Execute( yLine );
            }

            // 横線
            for (var j = 0; j <= canvasVm.MazeFrameData.SizeY; ++j) {
                var y = canvasVm.Margin + j * factor;
                var xLine = NewLine( 0d + canvasVm.Margin, y, canvasVm.Size - canvasVm.Margin, y, 1d );
                canvasVm.AddViewElementCommand?.Execute( xLine );
            }
        }

        void DrawWalls( MainWindowViewModel vm, MazeFrameViewModel canvasVm )
        {
            var wallLength = (canvasVm.Size - canvasVm.Margin * 2) / (double)canvasVm.MazeFrameData.SizeX;

            foreach (var position in canvasVm.MazeFrameData.WallPositions) {
                var p1 = position.Item1;
                var p2 = position.Item2;

                var wallLine = NewLine( p1.x1 * wallLength + canvasVm.Margin, p1.y1 * wallLength + canvasVm.Margin,
                    p2.x2 * wallLength + canvasVm.Margin, p2.y2 * wallLength + canvasVm.Margin,
                    5d );
                canvasVm.AddViewElementCommand?.Execute( wallLine );
            }

        }

        void DrawBoxes( MainWindowViewModel vm, MazeFrameViewModel canvasVm )
        {
        }

        Line NewLine( double x1, double y1, double x2, double y2, double thickness ) =>
            new Line { Stroke = Brushes.Black, StrokeThickness = thickness, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
    }
}
