using System.Windows;
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

            var wallLength = (canvasVm.Size - canvasVm.Margin * 2) / (double)canvasVm.MazeFrameData.SizeX;

            DrawFrame( vm, canvasVm, wallLength );
            DrawWalls( vm, canvasVm, wallLength );
            DrawBoxes( vm, canvasVm, wallLength );
        }

        void DrawFrame( MainWindowViewModel vm, MazeFrameViewModel canvasVm, double wallLength )
        {
            // 縦線
            for (var i = 0; i <= canvasVm.MazeFrameData.SizeX; ++i) {
                var x = canvasVm.Margin + i * wallLength;
                var yLine = NewLine( x, 0d + canvasVm.Margin, x, canvasVm.Size - canvasVm.Margin, 1d );
                canvasVm.AddViewElementCommand?.Execute( yLine );
            }

            // 横線
            for (var j = 0; j <= canvasVm.MazeFrameData.SizeY; ++j) {
                var y = canvasVm.Margin + j * wallLength;
                var xLine = NewLine( 0d + canvasVm.Margin, y, canvasVm.Size - canvasVm.Margin, y, 1d );
                canvasVm.AddViewElementCommand?.Execute( xLine );
            }
        }

        void DrawWalls( MainWindowViewModel vm, MazeFrameViewModel canvasVm, double wallLength )
        {
            foreach (var position in canvasVm.MazeFrameData.WallPositions) {
                var p1 = position.Item1;
                var p2 = position.Item2;

                var wallLine = NewLine( p1.x1 * wallLength + canvasVm.Margin, p1.y1 * wallLength + canvasVm.Margin,
                    p2.x2 * wallLength + canvasVm.Margin, p2.y2 * wallLength + canvasVm.Margin,
                    5d );
                canvasVm.AddViewElementCommand?.Execute( wallLine );
            }
        }

        void DrawBoxes( MainWindowViewModel vm, MazeFrameViewModel canvasVm, double wallLength )
        {
            // 赤箱
            if (canvasVm.MazeContentData.RedboxPosition.HasValue) {
                var redbox = NewBox( canvasVm.MazeContentData.RedboxPosition.Value, Brushes.Red, canvasVm.Margin, wallLength );
                canvasVm.AddViewElementCommand?.Execute( redbox );
            }

            // 黄箱
            if (canvasVm.MazeContentData.YellowboxPosition.HasValue) {
                var yellowbox = NewBox( canvasVm.MazeContentData.YellowboxPosition.Value, Brushes.Yellow, canvasVm.Margin, wallLength );
                canvasVm.AddViewElementCommand?.Execute( yellowbox );
            }

            // 緑箱
            foreach (var greenboxPosition in canvasVm.MazeContentData.GreenboxPositions) {
                var greenbox = NewBox( greenboxPosition, Brushes.Green, canvasVm.Margin, wallLength );
                canvasVm.AddViewElementCommand?.Execute( greenbox );
            }
        }

        Line NewLine( double x1, double y1, double x2, double y2, double thickness ) =>
            new Line { Stroke = Brushes.Black, StrokeThickness = thickness, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };

        Rectangle NewBox( (int x, int y) position, Brush fillBrush, double margin, double wallLength )
        {
            var origin = new Point {
                X = margin + position.x * wallLength + wallLength * 0.1d,
                Y = margin + position.y * wallLength + wallLength * 0.1d
            };
            return NewRectanble( fillBrush, wallLength * 0.8d, origin );
        }

        Rectangle NewRectanble( Brush fillBrush, double size, Point origin ) =>
            new Rectangle { Stroke = Brushes.Black, StrokeThickness = 1d, Fill = fillBrush, Width = size, Height = size, Tag = origin };
    }
}
