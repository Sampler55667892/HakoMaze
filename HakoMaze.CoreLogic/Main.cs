using System.Linq;
using System.Collections.Generic;
using FakeFrame;
using HakoMaze.Data;
using LegendUtil = HakoMaze.Data.MazeMapLegendUtility;

namespace HakoMaze.CoreLogic
{
    public class Main : SenderBase<object>, IListener<string>
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

            Broadcast( ComputationMessageHeader.Message, "Begin Compute()" );

            var map = makeMazeMapLogic.MakeMazeMap( frame, content );

            // 局面の圧縮 (箱の位置だけ)
            // R(x,y)Y(x,y)G(x1,y1)(x2,y2)...
            var compressedInitial = compressMapLogic.Compress( map );

            var counts = 0;
            MazeMapPosition reachedGoalMapPosition;

            var q = new Queue<MazeMapPosition>();
            q.Enqueue( new MazeMapPosition { Parent = null, Position = compressedInitial } );

            while (q.Count > 0) {
                // 処理のキャンセル通知のポーリング
                if ((counts++ % CheckCancelInterval) == 0) {
                    if (cancels) {
                        Broadcast( ComputationMessageHeader.Message, "Cancel Compute()" );
                        return 1;
                    }
                }

                Expand( q, out reachedGoalMapPosition, counts );
                if (reachedGoalMapPosition != null) {
                    // ルートの逆探索
                    var mapPositionLinks = MazeMapHistory.GetMapPositionLinks( reachedGoalMapPosition.Position, true );
                    Broadcast( ComputationMessageHeader.Message, "ゴールに至るルートを発見" );
                    Broadcast( ComputationMessageHeader.RouteToGoal, mapPositionLinks );
                    break;
                }
            }

            Broadcast( ComputationMessageHeader.Message, "End Compute()" );

            return 0;
        }

        public void Listen( ObjectMessage<string> message )
        {
            if (string.Compare( message.Content, "Cancel" ) == 0)
                cancels = true;
        }

        // counts -> デバッグ用
        // true -> ゴールに到達, false -> それ以外
        void Expand( Queue<MazeMapPosition> q, out MazeMapPosition reachedGoalMapPosition, int counts )
        {
            var currentMapPosition = q.Dequeue();

            // 圧縮局面の解凍
            var expandedContent = expandMapLogic.Expand( currentMapPosition.Position );
            var expandedMap = makeMazeMapLogic.MakeMazeMap( Frame, expandedContent );

            // 詰み判定 (黄箱が動かせる状態か？)
            //...

            // 赤箱の稼働範囲の計算
            var turningPreMovePositions = new List<((int x, int y), (int x, int y))>();
            var redboxPosition = (x:expandedContent.RedboxPosition.Value.x * 2 + 1, y:expandedContent.RedboxPosition.Value.y * 2 + 1);
            searchMovableAreaLogic.MarkRedboxMovableArea( redboxPosition, expandedMap, turningPreMovePositions );

            // 終了条件の判定
            if (DetectReachedGoal( expandedContent, expandedMap )) {
                reachedGoalMapPosition = currentMapPosition;
                return;
            } else
                reachedGoalMapPosition = null;

            // 局面の評価 (マニュアル)
            // 詰み (派生不可)
            if (!turningPreMovePositions.Any())
                return;

            // マーキングのクリア
            expandedMap = null; // GCに通知
            expandedMap = makeMazeMapLogic.MakeMazeMap( Frame, expandedContent );

            // 局面の展開
            for (var i = 0; i < turningPreMovePositions.Count; ++i) {
                var preMovePosition = turningPreMovePositions[ i ].Item1;
                var willMoveVector = turningPreMovePositions[ i ].Item2;

                // 箱をマップ外に出すことを考えると少し効率化されるかも...
                var movedYellowOrGreenPosition = (x:preMovePosition.x + willMoveVector.x * 2, y:preMovePosition.y + willMoveVector.y * 2);
                if (movedYellowOrGreenPosition.x <= 0 || expandedMap.GetLength( 0 ) <= movedYellowOrGreenPosition.x)
                    continue;
                if (movedYellowOrGreenPosition.y <= 0 || expandedMap.GetLength( 0 ) <= movedYellowOrGreenPosition.y)
                    continue;

                // マップを元に戻すためにセーブ (赤箱の移動前/移動後・赤箱の移動に伴う黄or緑箱の移動)
                // 可読性よりも効率優先
                var mapContentMovedYellowOrGreen = expandedMap[ preMovePosition.x + willMoveVector.x * 2, preMovePosition.y + willMoveVector.y * 2 ];
                var mapContentMovedRedbox = expandedMap[ preMovePosition.x + willMoveVector.x, preMovePosition.y + willMoveVector.y ];
                var mapContentPreMoveRedbox = expandedMap[ redboxPosition.x, redboxPosition.y ];

                ExpandCore( q, currentMapPosition, redboxPosition, expandedMap, preMovePosition, willMoveVector );

                // マップ (2次元配列) のコピーを行わずに，expandedMap を元に戻す
                expandedMap[ redboxPosition.x, redboxPosition.y ] = mapContentPreMoveRedbox;
                expandedMap[ preMovePosition.x + willMoveVector.x, preMovePosition.y + willMoveVector.y ] = mapContentMovedRedbox;
                expandedMap[ preMovePosition.x + willMoveVector.x * 2, preMovePosition.y + willMoveVector.y * 2 ] = mapContentMovedYellowOrGreen;
            }
        }

        void ExpandCore( Queue<MazeMapPosition> q, MazeMapPosition currentMapPosition, (int x, int y) redboxPosition,
            int[,] expandedMap, (int x, int y) preMovePosition, (int x, int y) willMoveVector )
        {
            var mapContentMovedYellowOrGreen = 
                expandedMap[ preMovePosition.x + willMoveVector.x * 2, preMovePosition.y + willMoveVector.y * 2 ];

            // 稼働範囲内の箱A (黄箱 or 緑箱) を指定方向に移動
            expandedMap[ preMovePosition.x + willMoveVector.x * 2, preMovePosition.y + willMoveVector.y * 2 ] =
                expandedMap[ preMovePosition.x + willMoveVector.x, preMovePosition.y + willMoveVector.y ];
            // 移動先にゴールがあったら上書きされないようにする
            if (LegendUtil.Matches( mapContentMovedYellowOrGreen, MazeMapLegend.Goal ))
                expandedMap[ preMovePosition.x + willMoveVector.x * 2, preMovePosition.y + willMoveVector.y * 2 ] |= MazeMapLegend.Goal;
            // 赤箱を箱Aの場所に移動
            expandedMap[ preMovePosition.x + willMoveVector.x, preMovePosition.y + willMoveVector.y ] = MazeMapLegend.Redbox;
            // 3座標の中に同じ座標が2つ含まれている場合あり
            if (LegendUtil.Matches( expandedMap[ redboxPosition.x, redboxPosition.y ], MazeMapLegend.Redbox ))
                expandedMap[ redboxPosition.x, redboxPosition.y ] = MazeMapLegend.Space;

            // 派生
            var nextPosition = compressMapLogic.Compress( expandedMap );
            // 新規マップなら履歴に追加 (マップ間のリンクも張る)
            var nextMapPosition = MazeMapHistory.Add( nextPosition, currentMapPosition );
            if (nextMapPosition != null)
                q.Enqueue( nextMapPosition );
        }

        // 前提：expandedMap は稼働範囲をマーク済み
        // true -> 黄箱がゴール地点にある かつ 黄箱を赤箱で押せる状態にある, false -> それ以外
        bool DetectReachedGoal( MazeContentData expandedContent, int[,] expandedMap )
        {
            if (Frame.GoalPosition.Value.x != expandedContent.YellowboxPosition.Value.x ||
                Frame.GoalPosition.Value.y != expandedContent.YellowboxPosition.Value.y)
                return false;

            // 黄箱が赤箱の稼働範囲内にある
            var yellowboxPosition = (x:expandedContent.YellowboxPosition.Value.x * 2 + 1, y:expandedContent.YellowboxPosition.Value.y * 2 + 1);
            if (!LegendUtil.Matches( expandedMap[ yellowboxPosition.x, yellowboxPosition.y ], MazeMapLegend.Marked ))
                return false;

            return true;
        }
    }
}
