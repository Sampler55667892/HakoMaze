using System.Threading.Tasks;
using System.Windows;
using FakeFrame;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class AutoSearchCommand : MainWindowCommand, IListener<string>
    {
        public AutoSearchCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override async void OnInitialize()
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

            // 自動検索の実行確認
            var result = MessageBox.Show( "自動検索 (マニュアルルール) を開始します", "確認", MessageBoxButton.OKCancel );
            if (result == MessageBoxResult.Cancel) {
                Exits = true;
                return;
            }

            AddHistoryMessage( "自動検索 (マニュアルルール) を開始" );

            await Compute();

            MessageBox.Show( "自動検索 (マニュアルルール) が完了しました", "確認", MessageBoxButton.OKCancel );
            AddHistoryMessage( "自動検索 (マニュアルルール) を終了" );

            Exits = true;
        }

        async Task Compute()
        {
            var main = new CoreLogic.Main();
            main.Listeners.Add( this );

            await Task.Run(() => main.Compute( CanvasViewModel.MazeFrameData, CanvasViewModel.MazeContentData ));
        }

        // 計算状況の最低限の表示 (Header でハンドリング)
        public void Listen( ObjectMessage<string> message ) =>
            AddHistoryMessage( message.Content );
    }
}
