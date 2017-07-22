using System.Collections.Generic;
using HakoMaze.Data;
using LegendUtil = HakoMaze.Data.Utilities.MazeMapLegendUtility;
using Const = HakoMaze.CoreLogic.LogicConstraints;

namespace HakoMaze.CoreLogic
{
    public class CompressMapLogic
    {
        // FrameSize は 10 以下
        // map の座標は 10 * 2 + 1 = 19 以下
        // (x, y) で 10bit
        // sizeof( long ) で 8 byte → long 1つで 6箱 (4 bit余り)
        // 箱の並びは，赤箱 → 黄箱 → 緑箱1 → 緑箱2 → ...
        // R(x,y)Y(x,y)G(x1,y1)(x2,y2)...

        public ulong[] Compress( int[,] map )
        {
            var mapSize = map.GetLength( 0 );

            var redBox = (x:-1, y:-1);
            var yellowBox = (x:-1, y:-1);
            var greenBoxes = new List<(int x, int y)>();

            for (var i = 0; i < mapSize; ++i) {
                for (var j = 0; j < mapSize; ++j) {
                    var item = map[ i, j ];
                    if (LegendUtil.Matches( item, MazeMapLegend.Redbox ))
                        redBox = (x:i, y:j);
                    else if (LegendUtil.Matches( item, MazeMapLegend.Yellowbox ))
                        yellowBox = (x:i, y:j);
                    else if (LegendUtil.Matches( item, MazeMapLegend.Greenbox ))
                        greenBoxes.Add((x:i, y: j));
                }
            }

            return Compress( redBox, yellowBox, greenBoxes );
        }

        ulong[] Compress( (int x, int y) redBox, (int x, int y) yellowBox, List<(int x, int y)> greenBoxes )
        {
            var countBoxes = 2 + greenBoxes.Count;

            // 座標の最大値を元に計算
            // 1箱当たり (1～19)(5bit) × 2 -> 10bit以下
            // 6箱までで64bit (8byte) (1long) 以下
            var longLength = countBoxes / Const.CountBoxesPerULong +
                ((countBoxes % Const.CountBoxesPerULong == 0) ? 0 : 1);

            var compressed = new ulong[ longLength ];

            // どの箱をどこに割り当てるかの計算・割当て
            var count = 0;
            // 赤箱・黄箱の割り当て
            AssignValue( redBox, compressed, count++ );
            AssignValue( yellowBox, compressed, count++ );
            // 緑箱の割り当て
            greenBoxes.ForEach( x => AssignValue( x, compressed, count++ ) );

            return compressed;
        }

        void AssignValue( (int x, int y) position, ulong[] compressed, int count )
        {
            // 0: xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx
            // ...
            // i: xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx
            //    01234567 89
            //               012345 6789
            //                          0123 456789
            //                                     01 23456789
            //                                                 01234567 89
            //                                                            012345 6789 (4bit余り)

            var index = count / Const.CountBoxesPerULong;

            // 座標をbit列に埋込み
            compressed[ index ] |= (ulong)position.x << (count % Const.CountBoxesPerULong) * Const.CountBitsPerXY;
            compressed[ index ] |= (ulong)position.y << ((count % Const.CountBoxesPerULong) * Const.CountBitsPerXY + Const.CountBitsPerXorY);
        }
    }
}
