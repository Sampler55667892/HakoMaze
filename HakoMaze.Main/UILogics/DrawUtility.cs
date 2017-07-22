using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HakoMaze.Main
{
    public static class DrawUtility
    {
        public static Line NewWall( double x1, double y1, double x2, double y2, double thickness ) =>
            NewLine( x1, y1, x2, y2, thickness );

        public static Rectangle NewRedbox( (int x, int y) position, double margin, double wallLength ) =>
            NewBox( position, Brushes.Red, margin, wallLength );

        public static Rectangle NewYellowbox( (int x, int y) position, double margin, double wallLength ) =>
            NewBox( position, Brushes.Yellow, margin, wallLength );

        public static Rectangle NewGreenbox( (int x, int y) position, double margin, double wallLength ) =>
            NewBox( position, Brushes.Green, margin, wallLength );

        public static Image NewGoal( (int x, int y) position, double margin, double wallLength ) =>
            NewImage( position, margin, wallLength );

        static Line NewLine( double x1, double y1, double x2, double y2, double thickness ) =>
            new Line { Stroke = Brushes.Black, StrokeThickness = thickness, X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };

        static Rectangle NewBox( (int x, int y) position, Brush fillBrush, double margin, double wallLength )
        {
            var origin = new Point {
                X = margin + position.x * wallLength + wallLength * 0.1d,
                Y = margin + position.y * wallLength + wallLength * 0.1d
            };
            return NewRectanble( fillBrush, wallLength * 0.8d, origin );
        }

        static Rectangle NewRectanble( Brush fillBrush, double size, Point origin ) =>
            new Rectangle { Stroke = Brushes.Black, StrokeThickness = 1d, Fill = fillBrush, Width = size, Height = size, Tag = origin };

        static Image NewImage( (int x, int  y) position, double margin, double wallLength )
        {
           var origin = new Point {
                X = margin + position.x * wallLength + wallLength * 0.1d,
                Y = margin + position.y * wallLength + wallLength * 0.1d
            };
            // pack を使ったリソースの相対パス指定 (https://msdn.microsoft.com/ja-jp/library/aa970069.aspx)
            var uri = new Uri( "pack://application:,,,/Resources/Images/Goal.jpg" );
            return new Image { Source = new BitmapImage( uri ), Width = wallLength * 0.8d, Height = wallLength * 0.8d, Tag = origin };
        }
    }
}
