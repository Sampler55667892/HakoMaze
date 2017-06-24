using HakoMaze.Common;

namespace HakoMaze.ViewModels
{
    public class MazeFrameViewModel : ViewModelBase
    {
        int size;
        public int Size 
        {
            get { return size; }
            set {
                size = value;
                RaiseProperyChanged( "Size" );
            }
        }

        public int Margin { get; set; }

        // View側で設定
        public RelayCommand AddViewElementCommand { get; set; }

        // View側で設定
        public RelayCommand ClearViewElementCommand { get; set; }
    }
}
