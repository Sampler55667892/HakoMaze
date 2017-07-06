using HakoMaze.Data;
using HakoMaze.Main.ViewModels;

namespace HakoMaze.Main.Commands
{
    public class OpenSampleCommand : MainWindowCommand
    {
        public OpenSampleCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            CanvasViewModel.MazeFrameData.Load( CreateSampleMazeFrameData() );
            CanvasViewModel.MazeContentData.Load( CreateSampleMazeContentData() );

            UpdateRenderCanvas();
        }

        MazeFrameData CreateSampleMazeFrameData()
        {
            var data = new MazeFrameData();

            data.SizeX =
            data.SizeY = 6;

            foreach (var wallPosition in
                new[] {
                    ((0, 0), (1, 0)), ((1, 0), (2, 0)), ((2, 0), (3, 0)), ((3, 0), (4, 0)), ((4, 0), (5, 0)),
                    ((0, 0), (0, 1)), ((2, 0), (2, 1)), ((4, 0), (4, 1)), ((6, 0), (6, 1)),
                    ((0, 1), (0, 2)), ((4, 1), (4, 2)), ((6, 1), (6, 2)),
                    ((1, 2), (2, 2)), ((2, 2), (3, 2)),
                    ((0, 2), (0, 3)), ((6, 2), (6, 3)), 
                    ((2, 3), (3, 3)), 
                    ((0, 3), (0, 4)), ((1, 3), (1, 4)), ((5, 3), (5, 4)), ((6, 3), (6, 4)),
                    ((2, 4), (3, 4)), ((3, 4), (4, 4)), ((4, 4), (5, 4)),
                    ((0, 4), (0, 5)), ((1, 4), (1, 5)), ((2, 4), (2, 5)), ((6, 4), (6, 5)),
                    ((0, 5), (0, 6)), ((6, 5), (6, 6)),
                    ((1, 6), (2, 6)), ((2, 6), (3, 6)), ((3, 6), (4, 6)), ((4, 6), (5, 6)), ((5, 6), (6, 6)),
                })
                data.AddWallPosition( wallPosition );

            return data;
        }

        MazeContentData CreateSampleMazeContentData()
        {
            var data = new MazeContentData();

            data.RedboxPosition = (5, 0);
            data.YellowboxPosition = (4, 1);

            foreach (var position in new[] { (2, 1), (0, 2), (4, 2), (5, 2), (3, 5) })
                data.AddGreenbox( position );

            return data;
        }
    }
}
