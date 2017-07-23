using System.Collections.Generic;

namespace HakoMaze.Data
{
    public class MazeMapPosition
    {
        public MazeMapPosition Parent { get; set; }
        public ulong[] Position { get; set; }

        public List<MazeMapPosition> GetLinksToRoot( bool startsFromInitial )
        {
            var links = new List<MazeMapPosition>();

            AddLinkToParent( this, links );

            if (startsFromInitial)
                links.Reverse();

            return links;
        }

        void AddLinkToParent( MazeMapPosition currentPosition, List<MazeMapPosition> links )
        {
            if (currentPosition == null)
                return;

            links.Add( currentPosition );
            AddLinkToParent( currentPosition.Parent, links );
        }
    }
}
