using System.Collections.Generic;
using System.Linq;
using HakoMaze.Data;
using LegendUtil = HakoMaze.Data.MazeMapLegendUtility;

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
            if (LegendUtil.Matches( map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ], MazeMapLegend.HorizontalWall ) ||
                LegendUtil.Matches( map[ nextPositions[ 0 ].x, nextPositions[ 0 ].y ], MazeMapLegend.VerticalWall )) {
                return false;
            }

            // 移動方向に箱があった場合
            if (LegendUtil.Matches( map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ], MazeMapLegend.Yellowbox ) ||
                LegendUtil.Matches( map[ nextPositions[ 1 ].x, nextPositions[ 1 ].y ], MazeMapLegend.Greenbox )) {
                // 箱の隣に壁があれば移動不可
                if (LegendUtil.Matches( map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ], MazeMapLegend.HorizontalWall ) ||
                    LegendUtil.Matches( map[ nextPositions[ 2 ].x, nextPositions[ 2 ].y ], MazeMapLegend.VerticalWall )) {
                    return false;
                }
                // 箱の隣が領域外でなく，かつ，箱の隣に箱があれば移動不可
                if ((0 <= nextPositions[ 3 ].x && 0 <= nextPositions[ 3 ].y) &&
                    (nextPositions[ 3 ].x < mapSize && nextPositions[ 3 ].y < mapSize) &&
                    (LegendUtil.Matches( map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ], MazeMapLegend.Yellowbox ) ||
                    LegendUtil.Matches( map[ nextPositions[ 3 ].x, nextPositions[ 3 ].y ], MazeMapLegend.Greenbox ))) {
                    return false;
                }
            }

            return true;
        }

        public void MarkRedboxMovableArea( (int x, int y) redboxPosition, int[,] map, List<((int x, int y), (int x, int y))> turningPreMovePositions ) =>
            MarkRedboxMovableArea( redboxPosition, map, turningPreMovePositions, (0, 0) );

        // 赤箱の稼働範囲をマーキング
        // 稼働範囲内の箱をマーキングしたとき，箱の座標と直前の移動方向を記録
        void MarkRedboxMovableArea( (int x, int y) redboxPosition, int[,] map, List<((int x, int y), (int x, int y))> turningPreMovePositions, (int x, int y) willMoveVector )
        {
            var mapSize = map.GetLength( 0 );

            // 稼働範囲の帰納的定義
            // 1. 赤箱の初期位置は稼働範囲内である
            // 2. 稼働範囲内のセルから移動可能なセルは稼働範囲内である

            map[ redboxPosition.x, redboxPosition.y ] |= MazeMapLegend.Marked;

            // 現座標が箱の上ならその先は移動しない
            if (LegendUtil.Matches( map[ redboxPosition.x, redboxPosition.y ], MazeMapLegend.Yellowbox ) ||
                LegendUtil.Matches( map[ redboxPosition.x, redboxPosition.y ], MazeMapLegend.Greenbox )) {
                // 現座標が箱の上のものだけを派生対象とする
                turningPreMovePositions.Add( ((redboxPosition.x - willMoveVector.x * 2, redboxPosition.y - willMoveVector.y * 2), (willMoveVector.x * 2, willMoveVector.y * 2)) );
                return;
            }

            for (var i = 0; i < moveVectors.Length; ++i) {
                var moveVector = moveVectors[ i ];
                var nextRedboxPosition = (x:redboxPosition.x + moveVector.x * 2, y:redboxPosition.y + moveVector.y * 2);

                // 移動方向が Frame アウトなら移動不可
                if (nextRedboxPosition.x < 0 || mapSize <= nextRedboxPosition.x)
                    continue;
                if (nextRedboxPosition.y < 0 || mapSize <= nextRedboxPosition.y)
                    continue;

                // 移動
                if (CanMove( redboxPosition, map, mapSize, moveVector)) {
                    // 移動方向がマーク済の場合
                    if (LegendUtil.Matches( map[ nextRedboxPosition.x, nextRedboxPosition.y ], MazeMapLegend.Marked )) {
                        // 移動先がマーク済だとしても派生可能な場合あり
                        // 移動先が箱の上である場合のみ記録 (無駄な派生の除去)
                        if (LegendUtil.Matches( map[ nextRedboxPosition.x, nextRedboxPosition.y ], MazeMapLegend.Yellowbox ) ||
                            LegendUtil.Matches( map[ nextRedboxPosition.x, nextRedboxPosition.y ], MazeMapLegend.Greenbox )) {
                            // turningPreMovePositions の重複チェック
                            if (!turningPreMovePositions.Any(
                                    x => x.Item1.x == redboxPosition.x &&
                                    x.Item1.y == redboxPosition.y &&
                                    x.Item2.x == moveVector.x * 2 &&
                                    x.Item2.y == moveVector.y * 2)) {
                                turningPreMovePositions.Add( (redboxPosition, (moveVector.x * 2, moveVector.y * 2)) );
                            }
                        }
                    } else
                        MarkRedboxMovableArea( nextRedboxPosition, map, turningPreMovePositions, moveVector );
                }
            }
        }
    }
}
