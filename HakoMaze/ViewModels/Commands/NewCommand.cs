using System.Windows.Media;
using System.Windows.Shapes;
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

            QuerySize();
            ShowFrame();
        }

        void QuerySize()
        {
            var sizeDialog = new SizeSettingDialog();
            sizeDialog.ShowDialog();    // スレッドをブロック
            if (sizeDialog.DataContext is SizeSettingDialogViewModel sizeVm) {
                ViewModel.MazeFrameData.SizeX = sizeVm.Size;
                ViewModel.MazeFrameData.SizeY = sizeVm.Size;
            }
        }

        void ShowFrame() => new DrawMazeFrameLogic().Draw( ViewModel );
    }
}
