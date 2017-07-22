namespace HakoMaze.Data.Utilities
{
    public static class MazeMapLegendUtility
    {
        public static bool Matches( int value, int legend ) => (value & legend) == legend;
    }
}
