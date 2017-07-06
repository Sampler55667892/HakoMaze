using System;
using System.Collections.Generic;
using System.Linq;
using FakeFrame;

namespace HakoMaze.Data
{
    // 非圧縮データ
    [Serializable]
    public class MazeContentData : ICloneable<MazeContentData>
    {
        List<(int x, int y)> greenboxPositions = new List<(int x, int y)>();

        public (int x, int y)? RedboxPosition { get; set; }

        public (int x, int y)? YellowboxPosition { get; set; }

        // Serialize 用に List
        public List<(int x, int y)> GreenboxPositions => greenboxPositions;

        public bool ExistsGreenboxPosition( (int x, int y) position ) => greenboxPositions.Contains( position );

        public bool IsValid =>
            RedboxPosition.HasValue &&
            YellowboxPosition.HasValue &&
            GreenboxPositions.Any();

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

        public bool UpdateGreenbox( int index, (int x, int y) position )
        {
            if (index < 0 || greenboxPositions.Count <= index)
                return false;

            greenboxPositions[ index ] = position;
            return true;
        }

        public void ClearGreenboxes() => greenboxPositions.Clear();

        public void ClearAllBoxes()
        {
            RedboxPosition = null;
            YellowboxPosition = null;
            ClearGreenboxes();
        }

        public void Load( MazeContentData data )
        {
            ClearAllBoxes();

            RedboxPosition = data.RedboxPosition;
            YellowboxPosition = data.YellowboxPosition;

            if (data.GreenboxPositions != null) {
                foreach (var position in data.GreenboxPositions)
                    GreenboxPositions.Add( position );
            }
        }

        public override string ToString()
        {
            if (!IsValid)
                return "Invalid Data";

            // 赤箱の位置/黄箱の位置/緑箱の位置
            return $"R={RedboxPosition.Value}/Y={YellowboxPosition.Value}/G={string.Join(",", GreenboxPositions.Select(x => x.ToString()))}";
        }

        public MazeContentData Clone()
        {
            var cloned = new MazeContentData
            {
                RedboxPosition = this.RedboxPosition,
                YellowboxPosition = this.YellowboxPosition
            };

            foreach (var greenboxPosition in this.GreenboxPositions)
                cloned.GreenboxPositions.Add( greenboxPosition );

            return cloned;
        }
    }
}
