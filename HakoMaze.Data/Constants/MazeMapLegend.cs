namespace HakoMaze.Data
{
    public class MazeMapLegend
    {
        public const int Space = 0;
        public const int HorizontalWall = 1;
        public const int VerticalWall = 2;
        public const int Redbox = 4;
        public const int Yellowbox = 8;
        public const int Greenbox = 16;
        // 稼働範囲の計算用
        public const int Marked = 32;
    }
}
