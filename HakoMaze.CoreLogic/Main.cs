using System.Linq;
using System.Collections.Generic;
using FakeFrame;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class Main : SenderBase<string>, IListener<string>
    {
        const int CheckCancelInterval = 100;

        MakeMazeMapLogic makeMazeMapLogic = new MakeMazeMapLogic();
        CompressMapLogic compressMapLogic = new CompressMapLogic();
        ExpandMapLogic expandMapLogic = new ExpandMapLogic();
        SearchMovableAreaLogic searchMovableAreaLogic = new SearchMovableAreaLogic();
        bool cancels = false;

        // TODO: 実装
        public int Compute( MazeFrameData frame, MazeContentData content )
        {
            Broadcast( null, "Begin Compute()" );

            var map = makeMazeMapLogic.MakeMazeMap( frame, content );

            // 局面の圧縮 (箱の位置だけ)
            // R(x,y)Y(x,y)G(x1,y1)(x2,y2)...
            var compressedInitial = compressMapLogic.Compress( map );

            // TODO: 局面の記録用のデータ生成
            //...

            var counts = 0;

            var q = new Queue<ulong[]>();
            q.Enqueue( compressedInitial );
            while (q.Count > 0) {
                // 処理のキャンセル通知のポーリング
                if ((counts++ % CheckCancelInterval) == 0) {
                    if (cancels) {
                        Broadcast( null, "Cancel Compute()" );
                        return 1;
                    }
                }

                var compressed = q.Dequeue();

                // 圧縮局面の解凍
                var expanded = expandMapLogic.Expand( compressed );
                var expandMap = makeMazeMapLogic.MakeMazeMap( frame, expanded );

                // 赤箱の稼働範囲の計算
                var turningPositions = new List<((int x, int y), (int x, int y))>();
                var redboxPosition = (expanded.RedboxPosition.Value.x * 2 + 1, expanded.RedboxPosition.Value.y * 2 + 1);
                searchMovableAreaLogic.MarkRedboxMovableArea( redboxPosition, expandMap, turningPositions );

                // 局面の評価 (マニュアル)
                // 詰み (派生不可)
                if (!turningPositions.Any())
                    continue;
                // (詰みの予測)
                //...
                // 終了条件の判定
                //...

                // 局面の展開
                for (var i = 0; i < turningPositions.Count; ++i) {
                    var turningPosition = turningPositions[ i ];

                    // ※マップコピーなしで Compress
                    //var nextMap = compressMapLogic.Compress()
                    //...
                }
            }

            Broadcast( null, "End Compute()" );

            return 0;
        }

        public void Listen( ObjectMessage<string> message )
        {
            if (string.Compare( message.Content, "Cancel" ) == 0)
                cancels = true;
        }
    }
}
