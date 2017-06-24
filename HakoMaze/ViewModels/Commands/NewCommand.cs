using HakoMaze.Views;
using HakoMaze.Logics;

namespace HakoMaze.ViewModels
{
    public class NewCommand : MainWindowCommand
    {
        public NewCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            ClearWalls();
            ClearBoxes();
            QuerySize();
            ShowFrame();
        }

        void ClearWalls() => ViewModel.CanvasViewModel.MazeFrameData.ClearWallPositions();

        void ClearBoxes() => ViewModel.CanvasViewModel.MazeContentData.ClearAllBoxes();

        void QuerySize()
        {
            var sizeDialog = new SizeSettingDialog();
            sizeDialog.ShowDialog();    // スレッドをブロック
            if (sizeDialog.DataContext is SizeSettingDialogViewModel sizeVm) {
                CanvasViewModel.MazeFrameData.SizeX = sizeVm.Size;
                CanvasViewModel.MazeFrameData.SizeY = sizeVm.Size;
            }
        }

        void ShowFrame() => new DrawMazeFrameLogic().Draw( ViewModel );
    }
}
