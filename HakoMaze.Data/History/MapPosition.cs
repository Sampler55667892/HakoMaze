using System.Collections.Generic;

namespace HakoMaze.Data
{
    public class MapPosition
    {
        public MapPosition Parent { get; set; }
        public ulong[] Position { get; set; }

        public List<MapPosition> GetLinksToRoot()
        {
            var links = new List<MapPosition>();

            AddLinkToParent( this, links );

            return links;
        }

        void AddLinkToParent( MapPosition currentPosition, List<MapPosition> links )
        {
            if (currentPosition == null)
                return;

            links.Add( currentPosition );
            AddLinkToParent( currentPosition.Parent, links );
        }
    }
}
