using System.Linq;
using System.Windows;
using System.Windows.Input;
using HakoMaze.Main.Logics;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class ManualSearchCommand : MainWindowCommand
    {
        (int x, int y) InitialRedboxPosition { get; set; }
        (int x, int y) CurrentRedboxPosition { get; set; }

        public ManualSearchCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            // サイズの初期設定前
            if (CanvasViewModel.MazeFrameData.SizeX == 0 || CanvasViewModel.MazeFrameData.SizeY == 0) {
                MessageBox.Show( "フレームのサイズが 0 です" );
                Exits = true;
                return;
            }

            if (!CanvasViewModel.MazeContentData.IsValid) {
                MessageBox.Show( "赤箱・黄箱・緑箱が揃っていません" );
                Exits = true;
                return;
            }

            TreeViewModel.Visibility = Visibility.Visible;

            InitialRedboxPosition =
            CurrentRedboxPosition = CanvasViewModel.MazeContentData.RedboxPosition.Value;

            TreeViewModel.Items.Add( CanvasViewModel.MazeContentData.Clone() );
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            TreeViewModel.Visibility = Visibility.Hidden;
            TreeViewModel.Items.Clear();
        }

        public override void OnKey()
        {
            base.OnKey();

            if (ModifierKeys == ModifierKeys.Control && Key == Key.Z) {
                Undo();
                return;
            }

            // 黄箱か0個 or 緑箱が0個なら何もしない (UNDO は実行可能)
            if (!CanvasViewModel.MazeContentData.IsValid)
                return;

            // 移動可能なら TreeView に項目を追加して移動する
            var moveVector = GetMoveVector( Key );
            if (moveVector.x == 0 && moveVector.y == 0)
                return;
            if (CanMoveRedbox( CurrentRedboxPosition, moveVector ))
                MoveRedbox( CurrentRedboxPosition, moveVector );
        }

        bool CanMoveRedbox( (int x, int y) currentRedboxPosition, (int x, int y) moveVector )
        {
            var searchLogic = new SearchMovableAreaLogic();
            // TreeViewModel のデータから現在の局面を取得
            return searchLogic.CanMove( CanvasViewModel.MazeFrameData, CanvasViewModel.MazeContentData, moveVector );
        }

        void MoveRedbox( (int x, int y) currentRedboxPosition, (int x, int y) moveVector )
        {
            var last = TreeViewModel.Items.LastOrDefault();
            var cloned = last.Clone();
            var boxCoordinateSize = (x:CanvasViewModel.MazeFrameData.SizeX, y:CanvasViewModel.MazeFrameData.SizeY);

            // 赤箱の現在座標の更新
            cloned.RedboxPosition = (last.RedboxPosition.Value.x + moveVector.x, last.RedboxPosition.Value.y + moveVector.y);
            if (cloned.RedboxPosition.Value.x == cloned.YellowboxPosition.Value.x &&
                cloned.RedboxPosition.Value.y == cloned.YellowboxPosition.Value.y) {
                // 赤箱から黄箱への相互作用
                var nextYellowbox = (x:last.RedboxPosition.Value.x + moveVector.x * 2, y:last.RedboxPosition.Value.y + moveVector.y * 2);
                // 領域外であれば黄箱を削除
                if ((0 <= nextYellowbox.x && 0 <= nextYellowbox.y) &&
                    (nextYellowbox.x < boxCoordinateSize.x && nextYellowbox.y < boxCoordinateSize.y)) {
                    cloned.YellowboxPosition = nextYellowbox;
                } else
                    cloned.YellowboxPosition = null;
            } else {
                // 赤箱から緑箱への相互作用
                for (var i = 0; i < cloned.GreenboxPositions.Count; ++i) {
                    var greenbox = cloned.GreenboxPositions[ i ];
                    if (cloned.RedboxPosition.Value.x == greenbox.x &&
                        cloned.RedboxPosition.Value.y == greenbox.y) {
                        // 領域外であれば緑箱を削除
                        var nextGreenbox = (x:last.RedboxPosition.Value.x + moveVector.x * 2, y:last.RedboxPosition.Value.y + moveVector.y * 2);
                        if ((0 <= nextGreenbox.x && 0 <= nextGreenbox.y) &&
                            (nextGreenbox.x < boxCoordinateSize.x && nextGreenbox.y < boxCoordinateSize.y)) {
                            cloned.UpdateGreenbox( i, nextGreenbox );
                        } else
                            cloned.DeleteGreenbox( greenbox );
                        break;
                    }
                }
            }

            TreeViewModel.Items.Add( cloned );

            // 再描画
            CanvasViewModel.MazeContentData.Load( cloned );
            CanvasViewModel.UpdateRenderCommand?.Execute( null );
        }

        (int x, int y) GetMoveVector( Key key )
        {
            (int x, int y) d = (0, 0);

            switch (key) {
                case Key.Left: d.x = -1; break;
                case Key.Right: d.x = 1; break;
                case Key.Up: d.y = -1; break;
                case Key.Down: d.y = 1; break;
            }

            return d;
        }

        void Undo()
        {
            if (TreeViewModel.Items.Count <= 1)
                return;

            // TreeView の現在の位置から1つ戻る
            TreeViewModel.Items.RemoveAt( TreeViewModel.Items.Count - 1 );
            var last = TreeViewModel.Items.LastOrDefault();
            var cloned = last.Clone();

            // 再描画
            CanvasViewModel.MazeContentData.Load( cloned );
            CanvasViewModel.UpdateRenderCommand?.Execute( null );
        }
    }
}
