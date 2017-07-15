using System.Collections.Generic;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class ExpandMapLogic
    {
        public MazeContentData Expand( ulong[] compressed )
        {
            // 赤箱 → 黄箱 → 緑箱1 → 緑箱2 → ...

            var positions = ConvertToPositions( compressed );

            var content = new MazeContentData();

            if (positions.Count > 0)
                content.RedboxPosition = positions[ 0 ];
            if (positions.Count > 1)
                content.YellowboxPosition = positions[ 1 ];
            for (var i = 2; i < positions.Count; ++i)
                content.AddGreenbox( positions[ i ] );

            return content;
        }

        List<(int x, int y)> ConvertToPositions( ulong[] compressed )
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

            var positions = new List<(int x, int y)>();

            for (var i = 0; i < compressed.Length; ++i) {
                var compressedOne = compressed[ i ];
                for (var j = 0; j < LogicConstraints.CountBoxesPerULong; ++j) {
                    var x = (int)((compressedOne >> (j * 10)) & 0b11111);
                    var y = (int)((compressedOne >> (j * 10 + 5)) & 0b11111);
                    // 最小座標は (1, 1)
                    if (x == 0 || y == 0)
                        break;
                    positions.Add( ((x - 1) / 2, (y - 1) / 2) );
                }
            }

            return positions;
        }
    }
}
