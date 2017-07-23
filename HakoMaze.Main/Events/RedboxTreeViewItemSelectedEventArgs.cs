using System;
using HakoMaze.Data;

namespace HakoMaze.Main
{
    public class RedboxTreeViewItemSelectedEventArgs : EventArgs
    {
        public MazeContentData Content { get; private set; }

        public RedboxTreeViewItemSelectedEventArgs( MazeContentData content ) => this.Content = content;
    }
}
