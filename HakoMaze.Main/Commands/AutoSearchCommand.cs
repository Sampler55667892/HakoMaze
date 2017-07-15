using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FakeFrame;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class AutoSearchCommand : MainWindowCommand, IListener<string>, ISender<string>
    {
        List<IListener<string>> listeners = new List<IListener<string>>();

        public ICollection<IListener<string>> Listeners => listeners;

        public AutoSearchCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            // サイズの初期設定前
            if (CanvasViewModel.IsFrameSizeZero) {
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

        public override void OnKey()
        {
            base.OnKey();

            if (Key == Key.Escape) {
                var result = MessageBox.Show( "自動検索 (マニュアルルール) をキャンセルします", "確認", MessageBoxButton.OKCancel );
                if (result == MessageBoxResult.OK)
                    Broadcast( new ObjectMessage<string>( string.Empty, "Cancel" ) );
            }
        }

        async Task Compute()
        {
            var main = new CoreLogic.Main();

            // 計算状況の受取り用
            main.Listeners.Add( this );

            // 処理のキャンセル通知用
            this.Listeners.Add( main );

            await Task.Run(() => main.Compute( CanvasViewModel.MazeFrameData, CanvasViewModel.MazeContentData ));
        }

        // 計算状況の最低限の表示 (Header でハンドリング)
        public void Listen( ObjectMessage<string> message ) =>
            AddHistoryMessage( message.Content );

        public void Broadcast( ObjectMessage<string> message ) =>
            listeners.ForEach( x => x.Listen( message ) );
    }
}
