﻿using HakoMaze.Common;
using HakoMaze.Models;

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

        public MazeFrameData MazeFrameData { get; private set; }
        public MazeContentData MazeContentData { get; private set; }

        public int Margin { get; set; }

        // View側で設定
        public RelayCommand AddViewElementCommand { get; set; }

        // View側で設定
        public RelayCommand ClearViewElementCommand { get; set; }

        public MazeFrameViewModel( MazeFrameData mazeFrameData, MazeContentData mazeContentData )
        {
            this.MazeFrameData = mazeFrameData;
            this.MazeContentData = mazeContentData;
        }
    }
}
