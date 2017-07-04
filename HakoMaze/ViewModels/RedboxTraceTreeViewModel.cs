using System.Collections.ObjectModel;
using System.Windows;
using FakeFrame;
using HakoMaze.Models;

namespace HakoMaze.ViewModels
{
    public class RedboxTraceTreeViewModel : ViewModelBase
    {
        Visibility visibility;
        public Visibility Visibility
        {
            get { return visibility; }
            set {
                visibility = value;
                RaiseProperyChanged( "Visibility" );
            }
        }

        public ObservableCollection<MazeContentData> Items { get; } = new ObservableCollection<MazeContentData>();
    }
}
