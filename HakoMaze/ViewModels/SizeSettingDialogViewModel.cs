using FakeFrame;

namespace HakoMaze.ViewModels
{
    public class SizeSettingDialogViewModel : ViewModelBase
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
    }
}
