using System.Windows;
using HakoMaze.Data;
using HakoMaze.Main.Views;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class NewCommand : MainWindowCommand
    {
        public NewCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            ClearFrame();
            ClearContent();
            QuerySize();

            UpdateRenderCanvas();
        }

        void ClearFrame() => ViewModel.CanvasViewModel.MazeFrameData.Clear();

        void ClearContent() => ViewModel.CanvasViewModel.MazeContentData.Clear();

        void QuerySize()
        {
            var sizeDialog = new SizeSettingDialog();
            sizeDialog.ShowDialog();    // スレッドをブロック
            if (sizeDialog.DataContext is SizeSettingDialogViewModel sizeVm) {
                if (sizeVm.Size < DataConstraints.MinFrameSize || DataConstraints.MaxFrameSize < sizeVm.Size)
                    MessageBox.Show( $"{DataConstraints.MinFrameSize} ～ {DataConstraints.MaxFrameSize} の範囲で入力して下さい", "error" );
                else {
                    CanvasViewModel.MazeFrameData.SizeX = sizeVm.Size;
                    CanvasViewModel.MazeFrameData.SizeY = sizeVm.Size;
                }
            }
        }
    }
}
