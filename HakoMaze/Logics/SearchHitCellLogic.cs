using System.Linq;
using System.Windows;

namespace HakoMaze.Logics
{
    public class SearchHitCellLogic
    {
        public (int x, int y)? Search( int frameSize, Point physicalPosition, double wallLength )
        {
            // Size = 5 の場合
            // ・─・─・─・─・─・
            // ｜　｜　｜　｜　｜　｜
            // ・─・─・─・─・─・
            // ｜　｜　｜　｜　｜　｜
            // ・─・─・─・─・─・
            // ｜　｜　｜　｜　｜　｜
            // ・─・─・─・─・─・
            // ｜　｜　｜　｜　｜　｜
            // ・─・─・─・─・─・
            // ｜　｜　｜　｜　｜　｜
            // ・─・─・─・─・─・

            // 1. 何行目のセルかを判定
            // 2. 何列目のセルかを判定

            var rowRanges = new (int i, int start, int end)[ frameSize ];
            for (var i = 0; i < frameSize; ++i)
                rowRanges[ i ] = (i, (int)(i * wallLength), (int)((i + 1) * wallLength));

            var hitYIndex = rowRanges.FirstOrDefault( y => y.start <= physicalPosition.Y && physicalPosition.Y <= y.end );
            if (hitYIndex.Equals( (0, 0, 0) ))
                return null;

            var columnRanges = new (int j, int start, int end)[ frameSize ];
            for (var j = 0; j < frameSize; ++j)
                columnRanges[ j ] = (j, (int)(j * wallLength), (int)((j + 1) * wallLength));

            var hitXIndex = columnRanges.FirstOrDefault( x => x.start <= physicalPosition.X && physicalPosition.X <= x.end );
            if (hitXIndex.Equals( (0, 0, 0) ))
                return null;

            return (hitXIndex.j, hitYIndex.i);
        }
    }
}
