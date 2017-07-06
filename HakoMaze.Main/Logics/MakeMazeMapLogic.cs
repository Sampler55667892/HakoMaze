using HakoMaze.Data;
using HakoMaze.Main.Constants;

namespace HakoMaze.Main.Logics
{
    public class MakeMazeMapLogic
    {
        public int [,] MakeMazeMap( MazeFrameData frameData, MazeContentData contentData )
        {
            // 箱座標系と壁座標系を1つの座標系にマップ
            // ・─・─・─・─・
            // ｜　｜　｜　｜　｜
            // ・─・─・─・─・
            // ｜　｜　｜　｜　｜
            // ・─・─・─・─・
            // ｜　｜　｜　｜　｜
            // ・─・─・─・─・
            // ｜　｜　｜　｜　｜
            // ・─・─・─・─・

            var map = new int[ frameData.SizeX * 2 + 1, frameData.SizeY * 2 + 1 ];

            // 壁埋め
            foreach (var wallPosition in frameData.WallPositions) {
                // 座標変換
                var x = 0;
                var y = 0;
                var p1 = wallPosition.Item1;
                var p2 = wallPosition.Item2;
                if (p1.x1 == p2.x2) {
                    // 縦壁
                    x = p1.x1 * 2;
                    y = p1.y1 * 2 + 1;
                    map[ x, y ] = MazeMapLegend.VerticalWall;
                } else {
                    // 横壁
                    x = p1.x1 * 2 + 1;
                    y = p1.y1 * 2;
                    map[ x, y ] = MazeMapLegend.HorizontalWall;
                }
            }

            // 箱埋め
            map[ contentData.RedboxPosition.Value.x * 2 + 1, contentData.RedboxPosition.Value.y * 2 + 1 ] = MazeMapLegend.Redbox;
            map[ contentData.YellowboxPosition.Value.x * 2 + 1, contentData.YellowboxPosition.Value.y * 2 + 1 ] = MazeMapLegend.Yellowbox;
            foreach (var greenbox in contentData.GreenboxPositions)
                map[ greenbox.x * 2 + 1, greenbox.y * 2 + 1 ] = MazeMapLegend.Greenbox;

            return map;
        }
    }
}
