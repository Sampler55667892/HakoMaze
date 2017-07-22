using FakeFrame;
using HakoMaze.Data;

namespace HakoMaze.Main.ViewModels
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

        public bool IsFrameSizeZero => MazeFrameData.SizeX == 0 || MazeFrameData.SizeY == 0;

        public MazeFrameData MazeFrameData { get; private set; }
        public MazeContentData MazeContentData { get; private set; }

        public int Margin { get; set; }

        // View側で設定
        public RelayCommand AddViewItemCommand { get; set; }
        // View側で設定
        public RelayCommand ClearViewItemsCommand { get; set; }

        public MazeFrameViewModel( MazeFrameData mazeFrameData, MazeContentData mazeContentData )
        {
            this.MazeFrameData = mazeFrameData;
            this.MazeContentData = mazeContentData;
        }

        #region UpdateRender

        public void UpdateRender()
        {
            if (IsFrameSizeZero)
                return;

            // 描画要素をクリア
            ClearViewItemsCommand?.Execute( null );

            var wallLength = (Size - Margin * 2) / (double)MazeFrameData.SizeX;

            UpdateFrame( wallLength ).
                UpdateWalls( wallLength ).
                UpdateBoxes( wallLength ).
                UpdateGoal( wallLength );
        }

        MazeFrameViewModel UpdateFrame( double wallLength )
        {
            // 縦線
            for (var i = 0; i <= MazeFrameData.SizeX; ++i) {
                var x = Margin + i * wallLength;
                var yWall = DrawUtility.NewWall( x, 0d + Margin, x, Size - Margin, 1d );
                AddViewItemCommand?.Execute( yWall );
            }

            // 横線
            for (var j = 0; j <= MazeFrameData.SizeY; ++j) {
                var y = Margin + j * wallLength;
                var xWall = DrawUtility.NewWall( 0d + Margin, y, Size - Margin, y, 1d );
                AddViewItemCommand?.Execute( xWall );
            }

            return this;
        }

        MazeFrameViewModel UpdateWalls( double wallLength )
        {
            foreach (var position in MazeFrameData.WallPositions) {
                var p1 = position.Item1;
                var p2 = position.Item2;

                var wall = DrawUtility.NewWall( p1.x1 * wallLength + Margin, p1.y1 * wallLength + Margin,
                    p2.x2 * wallLength + Margin, p2.y2 * wallLength + Margin,
                    5d );
                AddViewItemCommand?.Execute( wall );
            }

            return this;
        }

        MazeFrameViewModel UpdateBoxes( double wallLength )
        {
            // 赤箱
            if (MazeContentData.RedboxPosition.HasValue) {
                var redbox = DrawUtility.NewRedbox( MazeContentData.RedboxPosition.Value, Margin, wallLength );
                AddViewItemCommand?.Execute( redbox );
            }

            // 黄箱
            if (MazeContentData.YellowboxPosition.HasValue) {
                var yellowbox = DrawUtility.NewYellowbox( MazeContentData.YellowboxPosition.Value, Margin, wallLength );
                AddViewItemCommand?.Execute( yellowbox );
            }

            // 緑箱
            foreach (var greenboxPosition in MazeContentData.GreenboxPositions) {
                var greenbox = DrawUtility.NewGreenbox( greenboxPosition, Margin, wallLength );
                AddViewItemCommand?.Execute( greenbox );
            }

            return this;
        }

        MazeFrameViewModel UpdateGoal( double wallLength )
        {
            if (!MazeFrameData.GoalPosition.HasValue)
                return this;
            
            // ゴール上に箱があれば箱優先
            // 赤箱
            if (MazeContentData.RedboxPosition.HasValue &&
                MazeContentData.RedboxPosition.Value.x == MazeFrameData.GoalPosition.Value.x &&
                MazeContentData.RedboxPosition.Value.y == MazeFrameData.GoalPosition.Value.y) {
                return this;
            }
            // 黄箱
            if (MazeContentData.YellowboxPosition.HasValue &&
                MazeContentData.YellowboxPosition.Value.x == MazeFrameData.GoalPosition.Value.x &&
                MazeContentData.YellowboxPosition.Value.y == MazeFrameData.GoalPosition.Value.y) {
                return this;
            }
            // 緑箱
            foreach (var greenbox in MazeContentData.GreenboxPositions) {
                if (greenbox.x == MazeFrameData.GoalPosition.Value.x &&
                    greenbox.y == MazeFrameData.GoalPosition.Value.y) {
                    return this;
                }
            }

            var goal = DrawUtility.NewGoal( MazeFrameData.GoalPosition.Value, Margin, wallLength );
            AddViewItemCommand?.Execute( goal );

            return this;
        }

        #endregion  // UpdateRender
    }
}
