﻿using System;
using System.IO;
using System.Text;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public static class _DebugDump
    {
        static string Timestamp => DateTime.Now.ToString( "yyyyMMdd_HHmmss" );

        public static void DumpToDesktop( int[,] map, int counts = -1 ) => Dump( $"{Environment.GetFolderPath( Environment.SpecialFolder.Desktop )}\\_dump_{Timestamp}.txt", map, counts );

        public static void Dump( string filePath, int[,] map, int counts = -1 )
        {
            using (var fs = new FileStream( filePath, FileMode.CreateNew, FileAccess.Write ))
            using (var sw = new StreamWriter( fs, Encoding.UTF8 ))
            {
                sw.WriteLine( $"counts = {counts}" );

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
                case (MazeMapLegend.Space | MazeMapLegend.Marked): return "■";
                case (MazeMapLegend.Redbox | MazeMapLegend.Marked): return "Ⓡ";
                case (MazeMapLegend.Yellowbox | MazeMapLegend.Marked): return "Ⓨ";
                case (MazeMapLegend.Greenbox | MazeMapLegend.Marked): return "Ⓖ";
                case (MazeMapLegend.Goal | MazeMapLegend.Marked): return "Ⓔ";
                case (MazeMapLegend.Redbox | MazeMapLegend.Goal): return "ⓡ";
                case (MazeMapLegend.Yellowbox | MazeMapLegend.Goal): return "ⓨ";
                case (MazeMapLegend.Greenbox | MazeMapLegend.Goal): return "ⓖ";
                case MazeMapLegend.Space: return "　";
                case MazeMapLegend.HorizontalWall: return "─";
                case MazeMapLegend.VerticalWall: return "｜";
                case MazeMapLegend.Redbox: return "Ｒ";  
                case MazeMapLegend.Yellowbox: return "Ｙ";
                case MazeMapLegend.Greenbox: return "Ｇ";
                case MazeMapLegend.Goal: return "Ｅ";
            }

            return "　";
        }
    }
}
