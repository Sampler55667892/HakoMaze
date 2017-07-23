using System.Windows;
using System.Windows.Controls;
using FakeFrame;
using HakoMaze.Data;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Views
{
    public partial class RedboxTraceTree : UserControl
    {
        public RedboxTraceTree()
        {
            this.Loaded += RedboxTraceTree_Loaded;
            InitializeComponent();
        }

        void RedboxTraceTree_Loaded( object sender, RoutedEventArgs e )
        {
            this.Loaded -= RedboxTraceTree_Loaded;
            var tv = this.FindFirst<TreeView>();
            tv.SelectedItemChanged += Tv_SelectedItemChanged;
        }

        void Tv_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            var tv = sender as TreeView;

            if (DataContext is RedboxTraceTreeViewModel vm)
                vm.RaiseSelectedItemChangedEvent( tv.SelectedItem as MazeContentData );
        }
    }
}
