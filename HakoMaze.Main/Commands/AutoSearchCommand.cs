﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FakeFrame;
using HakoMaze.Data;
using HakoMaze.CoreLogic;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class AutoSearchCommand : MainWindowCommand, IListener<object>, ISender<string>
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

            if (!CanvasViewModel.MazeFrameData.GoalPosition.HasValue) {
                MessageBox.Show( "ゴールが設定されていません" );
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

            MessageBox.Show( "自動検索 (マニュアルルール) が完了しました", "確認", MessageBoxButton.OK );
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            TreeViewModel.RedboxTreeViewItemSelected -= TreeViewModel_RedboxTreeViewItemSelected;

            TreeViewModel.Visibility = Visibility.Hidden;
            TreeViewModel.Items.Clear();
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

        // 計算状況の表示 + 経路情報の受取り (Header でハンドリング)
        public void Listen( ObjectMessage<object> message )
        {
            if (message.Header == ComputationMessageHeader.Message)
                AddHistoryMessage( message.Content as string );
            else if (message.Header == ComputationMessageHeader.RouteToGoal) {
                var mapPositionList = message.Content as List<MazeMapPosition>;
                AddToTreeView( mapPositionList );
            }
        }

        public void Broadcast( ObjectMessage<string> message ) =>
            listeners.ForEach( x => x.Listen( message ) );

        void AddToTreeView( List<MazeMapPosition> mapPositionList )
        {
            if (mapPositionList == null)
                return;

            var expandMapLogic = new ExpandMapLogic();
            for (var i = 0; i < mapPositionList.Count; ++i) {
                var expandedMap = expandMapLogic.Expand( mapPositionList[ i ].Position );
                // 非同期スレッドからUI要素にアクセス -> Dispatcher を使う
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TreeViewModel.Items.Add( expandedMap );
                    TreeViewModel.Visibility = Visibility.Visible;
                });
            }

            // TreeView 上の項目選択のハンドラを登録 (登録のインタフェースは TreeView のVMが提供)
            TreeViewModel.RedboxTreeViewItemSelected += TreeViewModel_RedboxTreeViewItemSelected;
        }

        void TreeViewModel_RedboxTreeViewItemSelected( object sender, RedboxTreeViewItemSelectedEventArgs args )
        {
            // 再描画
            CanvasViewModel.MazeContentData.Load( args.Content );
            CanvasViewModel.UpdateRender();
        }
    }
}
