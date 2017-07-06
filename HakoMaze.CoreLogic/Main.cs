using System.Collections.Generic;
using FakeFrame;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class Main : SenderBase<string>
    {
        // TODO: 実装
        public int Compute( MazeFrameData frame, MazeContentData content )
        {
            Broadcast( null, "Begin Compute()" );

            var map = new MakeMazeMapLogic().MakeMazeMap( frame, content );
            var mapSize = map.GetLength( 0 );

            // TODO: 局面の圧縮 (箱の位置だけ)
            // R(x,y)Y(x,y)G(x1,y1)(x2,y2)...
            // sizeof(long) は 8
            //...

            //var q = new Queue<>
            //q.Enqueue();

            // TODO: 局面の記録用のデータ生成
            //...
            // TODO: 赤箱の稼働範囲の計算
            //...

            Broadcast( null, "End Compute()" );

            return 0;
        }
    }
}
