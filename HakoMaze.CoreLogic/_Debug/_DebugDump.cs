using System.IO;
using System.Text;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public static class _DebugDump
    {
        public static void Dump( string filePath, int[,] map )
        {
            using (var fs = new FileStream( filePath, FileMode.CreateNew, FileAccess.Write ))
            using (var sw = new StreamWriter( fs, Encoding.UTF8 ))
            {
                var size = map.GetLength( 0 );
                for (var y = 0; y < size; ++y) {
                    var line = string.Empty;
                    for (var x = 0; x < size; ++x)
                        line += Replace( map[ x, y ] );
                    sw.WriteLine( line );
                }
            }
        }

        static string Replace( int item )
        {
            switch (item) {
                case MazeMapLegend.Space: return "　";
                case MazeMapLegend.HorizontalWall: return "─";
                case MazeMapLegend.VerticalWall: return "｜";
                case MazeMapLegend.Redbox: return "Ｒ";  
                case MazeMapLegend.Yellowbox: return "Ｙ";
                case MazeMapLegend.Greenbox: return "Ｇ";
            }

            return "　";
        }
    }
}
