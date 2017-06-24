using System.Collections.Generic;

namespace HakoMaze.Models
{
    // 非圧縮データ
    public class MazeContentData
    {
        List<(int x, int y)> greenboxPositions = new List<(int x, int y)>();

        public (int x, int y)? RedboxPosition { get; set; }

        public (int x, int y)? YellowboxPosition { get; set; }

        public List<(int x, int y)> GreenboxPositions => greenboxPositions;

        public bool ExistsGreenboxPosition( (int x, int y) position ) => greenboxPositions.Contains( position );

        public bool AddGreenbox( (int x, int y) position )
        {
            if (greenboxPositions.Contains( position ))
                return false;

            greenboxPositions.Add( position );
            return true;
        }

        public bool DeleteGreenbox( (int x, int y) position )
        {
            if (!greenboxPositions.Contains( position ))
                return false;

            greenboxPositions.Remove( position );
            return true;
        }

        public void ClearGreenboxes() => greenboxPositions.Clear();

        public void ClearAllBoxes()
        {
            RedboxPosition = null;
            YellowboxPosition = null;
            ClearGreenboxes();
        }
    }
}
