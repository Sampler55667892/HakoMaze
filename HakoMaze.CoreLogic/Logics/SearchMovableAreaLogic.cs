using System.Collections.Generic;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class SearchMovableAreaLogic
    {
        static (int x, int y)[] moveVectors = new[] {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };

        // MazeMap 上で計算
        public bool CanMove( MazeFrameData frameData, MazeContentData contentData, (int x, int y) moveVector )
        {
            var map = new MakeMazeMapLogic().MakeMazeMap( frameData, contentData );
            var mapSize = map.GetLength( 0 );

            // _Dump
            //var filePath = $"{Environment.GetFolderPath( Environment.SpecialFolder.Desktop )}\\_dump.txt";
            //_DebugDump.Dump( filePath, map );

            var redboxPosition = (x:contentData.RedboxPosition.Value.x * 2 + 1, y:contentData.RedboxPosition.Value.y * 2 + 1);
            return CanMove( redboxPosition, map, mapSize, moveVector );
        }

        bool CanMove( (int x, int y) redboxPosition, int[,] map, int mapSize, (int x, int y) moveVector )
        {
            var nextPositions = new (int x, int y)[ 4 ];
            for (var i = 0; i < nextPositions.Length; ++i)
                nextPositions[ i ] = (x:redboxPosition.x + moveVector.x * (i + 1), y:redboxPosition.y + moveVector.y * (i + 1));

            // 移動方向が Frame アウトなら移動不可
            if (nextPositions[ 1 ].x < 0 || mapSize <= nextPositions[ 1 ].x)
                return false;
            if (nextPositions[ 1 ].y < 0 || mapSize <= nextPositions[ 1 ].y)
                return false;

            // 移動方向に壁があれば移動不可
            if (Matches( map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ], MazeMapLegend.HorizontalWall ) ||
                Matches( map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ], MazeMapLegend.VerticalWall )) {
                return false;
            }

            // 移動方向に箱があった場合
            if (Matches( map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ], MazeMapLegend.Yellowbox ) ||
                Matches( map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ], MazeMapLegend.Greenbox )) {
                // 箱の隣に壁があれば移動不可
                if (Matches( map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ], MazeMapLegend.HorizontalWall ) ||
                    Matches( map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ], MazeMapLegend.VerticalWall )) {
                    return false;
                }
                // 箱の隣が領域外でなく，かつ，箱の隣に箱があれば移動不可
                if ((0 <= nextPositions[ 3 ].x && 0 <= nextPositions[ 3 ].y) &&
                    (nextPositions[ 3 ].x < mapSize && nextPositions[ 3 ].y < mapSize) &&
                    (Matches( map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ], MazeMapLegend.Yellowbox ) ||
                    Matches( map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ], MazeMapLegend.Greenbox ))) {
                    return false;
                }
            }

            return true;
        }

        bool Matches( int value, int legend ) => (value & legend) == legend;

        public void MarkRedboxMovableArea( (int x, int y) redboxPosition, int[,] map, List<((int x, int y), (int x, int y))> turningPositions ) =>
            MarkRedboxMovableArea( redboxPosition, map, turningPositions, (0, 0) );

        // 赤箱の稼働範囲をマーキング
        // 稼働範囲内の箱をマーキングしたとき，箱の座標と直前の移動方向を記録
        void MarkRedboxMovableArea( (int x, int y) redboxPosition, int[,] map, List<((int x, int y), (int x, int y))> turningPositions, (int x, int y) preMoveVector )
        {
            var mapSize = map.GetLength( 0 );

            // 稼働範囲の帰納的定義
            // 1. 赤箱の初期位置は稼働範囲内である
            // 2. 稼働範囲内のセルから移動可能なセルは稼働範囲内である

            map[ redboxPosition.x, redboxPosition.y ] |= MazeMapLegend.Marked;

            // 現座標が箱の上ならその先は移動しない
            if (Matches( map[ redboxPosition.x, redboxPosition.y ], MazeMapLegend.Yellowbox ) ||
                Matches( map[ redboxPosition.x, redboxPosition.y ], MazeMapLegend.Greenbox )) {
                turningPositions.Add( (redboxPosition, preMoveVector) );
                return;
            }

            for (var i = 0; i < moveVectors.Length; ++i) {
                var nextPosition = (x:redboxPosition.x + moveVectors[ i ].x * 2, y:redboxPosition.y + moveVectors[ i ].y * 2);

                // 移動方向が Frame アウトなら移動不可
                if (nextPosition.x < 0 || mapSize <= nextPosition.x)
                    continue;
                if (nextPosition.y < 0 || mapSize <= nextPosition.y)
                    continue;

                // 移動方向がマーク済なら何もしない
                if (Matches( map[ nextPosition.x, nextPosition.y ], MazeMapLegend.Marked ))
                    continue;

                // 移動
                if (CanMove( redboxPosition, map, mapSize, moveVectors[ i ]))
                    MarkRedboxMovableArea( nextPosition, map, turningPositions, moveVectors[ i ] );
            }
        }
    }
}
