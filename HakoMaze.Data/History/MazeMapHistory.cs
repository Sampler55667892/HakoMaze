using System;
using System.Collections.Generic;
using System.Linq;

namespace HakoMaze.Data
{
    // {Singleton}
    public static class MazeMapHistory
    {
        // C# Dictionary の内部実装 (https://csharptan.wordpress.com/2011/12/13/%E3%82%B3%E3%83%AC%E3%82%AF%E3%82%B7%E3%83%A7%E3%83%B3-2/)
        static Dictionary<string, List<MazeMapPosition>> allMapPosition = new Dictionary<string, List<MazeMapPosition>>();

        public static bool IsNewMap( ulong[] position ) => Find( position ) == null;

        public static MazeMapPosition Add( ulong[] position, MazeMapPosition parentMapPosition )
        {
            var key = MakeKey( position );
            if (!allMapPosition.ContainsKey( key )) {
                var newOne = new MazeMapPosition { Parent = parentMapPosition, Position = position };
                allMapPosition.Add( key, new List<MazeMapPosition> { newOne } );
                return newOne;
            }

            var positionList = allMapPosition[ key ];
            if (positionList == null || !positionList.Any())
                throw new Exception( "MapHistory.cs/Add() ; positionList == null || !positionList.Any()" );

            for (var i = 0; i < positionList.Count; ++i) {
                if (AreSame( positionList[ i ].Position, position ))
                    return null;
            }

            var newMapPosition = new MazeMapPosition { Parent = parentMapPosition, Position = position };
            positionList.Add( newMapPosition );

            return newMapPosition;
        }

        // 逆探索
        public static List<MazeMapPosition> GetMapPositionLinks( ulong[] position, bool startsFromInitial ) => Find( position )?.GetLinksToRoot( startsFromInitial );

        public static void Clear()
        {
            foreach (var key in allMapPosition.Keys) {
                var list = allMapPosition[ key ];
                list.Clear();
                list = null;
            }

            allMapPosition.Clear();
            allMapPosition = null;  // GCに通知
            allMapPosition = new Dictionary<string, List<MazeMapPosition>>();
        }

        static MazeMapPosition Find( ulong[] position )
        {
            var key = MakeKey( position );
            if (!allMapPosition.ContainsKey( key ))
                return null;

            var positionList = allMapPosition[ key ];
            if (positionList == null || !positionList.Any())
                throw new Exception( "MapHistory.cs/Find() ; positionList == null || !positionList.Any()" );

            for (var i = 0; i < positionList.Count; ++i) {
                var recordedPosition = positionList[ i ];
                if (AreSame( recordedPosition.Position, position ))
                    return recordedPosition;
            }

            return null;
        }

        static string MakeKey( ulong[] position )
        {
            var key = string.Empty;
            if (position == null)
                return key;

            foreach (var item in position)
                key += item.ToString();
            return key;
        }

        static bool AreSame( ulong[] a, ulong[] b )
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;
            for (var i = 0; i < a.Length; ++i) {
                if (a[ i ] != b[ i ])
                    return false;
            }
            return true;
        }
    }
}
