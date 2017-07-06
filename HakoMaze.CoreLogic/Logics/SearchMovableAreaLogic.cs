using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class SearchMovableAreaLogic
    {
        // MazeMap 上で計算
        public bool CanMove( MazeFrameData frameData, MazeContentData contentData, (int x, int y) moveVector )
        {
            var map = new MakeMazeMapLogic().MakeMazeMap( frameData, contentData );
            var mapSize = map.GetLength( 0 );

            // _Dump
            //var filePath = $"{Environment.GetFolderPath( Environment.SpecialFolder.Desktop )}\\_dump.txt";
            //_DebugDump.Dump( filePath, map );

            var currentPosition = (x:contentData.RedboxPosition.Value.x * 2 + 1, y:contentData.RedboxPosition.Value.y * 2 + 1);
            var nextPositions = new (int x, int y)[ 4 ];
            for (var i = 0; i < nextPositions.Length; ++i)
                nextPositions[ i ] = (x:currentPosition.x + moveVector.x * (i + 1), y:currentPosition.y + moveVector.y * (i + 1));

            // 移動方向が Frame アウトなら移動不可
            if (nextPositions[ 1 ].x < 0 || mapSize <= nextPositions[ 1 ].x)
                return false;
            if (nextPositions[ 1 ].y < 0 || mapSize <= nextPositions[ 1 ].y)
                return false;

            // 移動方向に壁があれば移動不可
            if (map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ] == MazeMapLegend.HorizontalWall ||
                map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ] == MazeMapLegend.VerticalWall) {
                return false;
            }

            // 移動方向に箱があった場合
            if (map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ] == MazeMapLegend.Yellowbox ||
                map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ] == MazeMapLegend.Greenbox) {
                // 箱の隣に壁があれば移動不可
                if (map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ] == MazeMapLegend.HorizontalWall ||
                    map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ] == MazeMapLegend.VerticalWall) {
                    return false;
                }
                // 箱の隣が領域外でなく，かつ，箱の隣に箱があれば移動不可
                if ((0 <= nextPositions[ 3 ].x && 0 <= nextPositions[ 3 ].y) &&
                    (nextPositions[ 3 ].x < mapSize && nextPositions[ 3 ].y < mapSize) &&
                    (map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ] == MazeMapLegend.Yellowbox ||
                    map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ] == MazeMapLegend.Greenbox)) {
                    return false;
                }
            }

            return true;
        }

        // TODO: 実装
        // 赤箱の稼働範囲をマーキング
        public void MarkRedboxMovableArea( int[,] map, int mapSize )
        {
        }
    }
}
