using FakeFrame;
using HakoMaze.Data;

namespace HakoMaze.CoreLogic
{
    public class Main : SenderBase<string>
    {
        // TODO: 実装
        public int Compute( MazeFrameData frame, MazeContentData content )
        {
            Broadcast( null, "Begin Compute()" );

            Broadcast( null, "End Compute()" );

            return 0;
        }
    }
}
