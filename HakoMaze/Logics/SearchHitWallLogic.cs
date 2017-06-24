using System.Linq;
using System.Windows;

namespace HakoMaze.Logics
{
    public class SearchHitWallLogic
    {
        public ((int x1, int y1), (int x2, int y2))? Search( int frameSize, Point physicalPosition, double wallLength, int pickMargin )
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

            // 次の分岐探索
            // 1. 全体を "横壁の集合" (Y) と "縦壁の集合" (T) に分ける
            // 2. "横壁の集合" を Size + 1 に分ける Y = (Y_{1}, Y_{2}, ...)
            // 3. "縦壁の集合" を Size + 1 に分ける T = (T_{1}, T_{2}, ...)
            // 4. "横壁の集合" のいずれかにヒットするかの判定 → ヒットする Y_{i} があれば詳細検索
            // 5. "縦壁の集合" のいずれかにヒットするかの判定 → ヒットする T_{i} があれば詳細検索

            var yRanges = new(int i, int start, int end)[ frameSize + 1 ];
            for (var i = 0; i <= frameSize; ++i)
                yRanges[i] = (i, (int)(i * wallLength - pickMargin), (int)(i * wallLength + pickMargin));

            var tRanges = new(int j, int start, int end)[ frameSize + 1 ];
            for (var j = 0; j <= frameSize; ++j)
                tRanges[j] = (j, (int)(j * wallLength - pickMargin), (int)(j * wallLength + pickMargin));

            // ヒットする Y 集合があるか
            var hitYSet = yRanges.FirstOrDefault( y => y.start <= physicalPosition.Y && physicalPosition.Y <= y.end );
            if (!hitYSet.Equals((0, 0, 0))) {
                var found = SearchYSet( frameSize, wallLength, hitYSet.i, physicalPosition.X );
                if (found.HasValue)
                    return found.Value;
            }

            // ヒットする T 集合があるか
            var hitTSet = tRanges.FirstOrDefault( t => t.start <= physicalPosition.X && physicalPosition.X <= t.end );
            if (!hitTSet.Equals((0, 0, 0))) {
                var found = SearchTSet( frameSize, wallLength, hitTSet.j, physicalPosition.Y );
                if (found.HasValue)
                    return found.Value;
            }

            return null;
        }

        ((int x1, int y1), (int x2, int y2))? SearchYSet( int frameSize, double wallLength, int hitIIndex, double physicalPositionX )
        {
            // ・─・─・─・─・─・
            var jRanges = new (int j, int start, int end)[ frameSize ];
            for (var j = 0; j < frameSize; ++j)
                jRanges[ j ] = (j, (int)(j * wallLength), (int)((j + 1) * wallLength));

            var hitJWall = jRanges.FirstOrDefault( j => j.start <= physicalPositionX && physicalPositionX <= j.end );

            return hitJWall.Equals((0, 0, 0)) ?
                (((int x1, int y1), (int x2, int y2))?)null :
                ((hitJWall.j, hitIIndex), (hitJWall.j + 1, hitIIndex));
        }

        ((int x1, int y1), (int x2, int y2))? SearchTSet( int frameSize, double wallLength, int hitJIndex, double physicalPositionY )
        {
            // ・
            // ｜
            // ・
            // ｜
            // ・
            // ｜
            // ・
            // ｜
            // ・
            // ｜
            // ・
            var iRanges = new (int i, int start, int end)[ frameSize ];
            for (var i = 0; i < frameSize; ++i)
                iRanges[ i ] = (i, (int)(i * wallLength), (int)((i + 1) * wallLength));

            var hitIWall = iRanges.FirstOrDefault( i => i.start <= physicalPositionY && physicalPositionY <= i.end );
            
            return hitIWall.Equals((0, 0, 0)) ?
                (((int x1, int y1), (int x2, int y2))?)null :
                ((hitJIndex, hitIWall.i), (hitJIndex, hitIWall.i + 1));
        }
    }
}
