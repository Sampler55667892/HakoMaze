using System;
using System.Collections.Generic;

namespace HakoMaze.Models
{
    // 非圧縮データ
    [Serializable]
    public class MazeFrameData
    {
        List<((int x1, int y1), (int x2, int y2))> wallPositions = new List<((int x1, int y1), (int x2, int y2))>();

        public int SizeX { get; set; }

        public int SizeY { get; set; }

        // 壁の位置の記録 (壁用には Zip/Unzip は使わない)
        // Serialize 用に List
        public List<((int x1, int y1), (int x2, int y2))> WallPositions => wallPositions;

        // 壁データ
        // 横壁
        // (i, j) -> (i + 1, j)
        // 竪壁
        // (i, j) -> (i, j + 1)

        public bool ExistsWallPosition( ((int x1, int y1), (int x2, int y2)) position ) => wallPositions.Contains( position );

        public bool AddWallPosition( ((int x1, int y1), (int x2, int y2)) position )
        {
            if (wallPositions.Contains( position ))
                return false;

            wallPositions.Add( position );
            return true;
        }

        public bool DeleteWallPosition( ((int x1, int y1), (int x2, int y2)) position )
        {
            if (!wallPositions.Contains( position ))
                return false;

            wallPositions.Remove( position );
            return true;
        }

        public void ClearWallPositions() => wallPositions.Clear();

        public void Load( MazeFrameData data )
        {
            this.SizeX = data.SizeX;
            this.SizeY = data.SizeY;

            this.ClearWallPositions();

            if (data.WallPositions != null) {
                foreach (var position in data.WallPositions)
                    this.AddWallPosition( position );
            }
        }
    }
}
