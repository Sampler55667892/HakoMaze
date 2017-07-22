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

        MazeFrameData Frame { get; set; }

        public int Compute( MazeFrameData frame, MazeContentData content )
        {
            this.Frame = frame;

            Broadcast( null, "Begin Compute()" );

            var map = makeMazeMapLogic.MakeMazeMap( frame, content );

            // 局面の圧縮 (箱の位置だけ)
            // R(x,y)Y(x,y)G(x1,y1)(x2,y2)...
            var compressedInitial = compressMapLogic.Compress( map );

            var counts = 0;

            var q = new Queue<MapPosition>();
            q.Enqueue( new MapPosition { Parent = null, Position = compressedInitial } );

            while (q.Count > 0) {
                // 処理のキャンセル通知のポーリング
                if ((counts++ % CheckCancelInterval) == 0) {
                    if (cancels) {
                        Broadcast( null, "Cancel Compute()" );
                        return 1;
                    }
                }

                Expand( q );
            }

            Broadcast( null, "End Compute()" );

            return 0;
        }

        public void Listen( ObjectMessage<string> message )
        {
            if (string.Compare( message.Content, "Cancel" ) == 0)
                cancels = true;
        }

        // TODO: 残実装
        void Expand( Queue<MapPosition> q )
        {
            var currentMapPosition = q.Dequeue();

            // 圧縮局面の解凍
            var expandedContent = expandMapLogic.Expand( currentMapPosition.Position );
            var expandedMap = makeMazeMapLogic.MakeMazeMap( Frame, expandedContent );

            // 詰み判定 (黄箱が動かせる状態か？)
            //...

            // 赤箱の稼働範囲の計算
            var turningPositions = new List<((int x, int y), (int x, int y))>();
            var redboxPosition = (x:expandedContent.RedboxPosition.Value.x * 2 + 1, y:expandedContent.RedboxPosition.Value.y * 2 + 1);
            searchMovableAreaLogic.MarkRedboxMovableArea( redboxPosition, expandedMap, turningPositions );

            // 局面の評価 (マニュアル)
            // 詰み (派生不可)
            if (!turningPositions.Any())
                return;
            // 終了条件の判定 (黄箱がゴール地点にある かつ 黄箱を赤箱で押せる状態にある)
            //...

            // 局面の展開
            for (var i = 0; i < turningPositions.Count; ++i) {
                var targetboxPosition = turningPositions[ i ].Item1;
                var moveVector = turningPositions[ i ].Item2;

                // マップを元に戻すためにセーブ
                var mapContentMoved = expandedMap[ targetboxPosition.x + moveVector.x, targetboxPosition.y + moveVector.y ];
                var mapContentTarget = expandedMap[ targetboxPosition.x, targetboxPosition.y ];
                var mapContentRedbox = expandedMap[ redboxPosition.x, redboxPosition.y ];

                // 稼働範囲内の箱A (黄箱 or 緑箱) を指定方向に移動
                expandedMap[ targetboxPosition.x + moveVector.x, targetboxPosition.y + moveVector.y ] =
                    expandedMap[ targetboxPosition.x, targetboxPosition.y ];
                // 赤箱を箱Aの場所に移動
                expandedMap[ targetboxPosition.x, targetboxPosition.y ] = MazeMapLegend.Redbox;
                expandedMap[ redboxPosition.x, redboxPosition.y ] = MazeMapLegend.Space;

                // 派生
                var nextPosition = compressMapLogic.Compress( expandedMap );
                // 新規マップなら履歴に追加 (マップ間のリンクも張る)
                var nextMapPosition = MapHistory.Add( nextPosition, currentMapPosition );
                if (nextMapPosition != null)
                    q.Enqueue( nextMapPosition );

                // マップ (2次元配列) のコピーを行わずに，expandedMap を元に戻す
                expandedMap[ redboxPosition.x, redboxPosition.y ] = mapContentRedbox;
                expandedMap[ targetboxPosition.x, targetboxPosition.y ] = mapContentTarget;
                expandedMap[ targetboxPosition.x + moveVector.x, targetboxPosition.y + moveVector.y ] = mapContentMoved;
            }
        }
    }
}
