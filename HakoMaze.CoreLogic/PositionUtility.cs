namespace HakoMaze.CoreLogic
{
    // メモリー節約のための圧縮用
    public static class PositionUtility
    {
        public static int Zip( int x, int y, int size ) => y * size + x;
        
        public static (int x, int y) Unzip( int position, int size ) => (position % size, position / size);
    }
}
