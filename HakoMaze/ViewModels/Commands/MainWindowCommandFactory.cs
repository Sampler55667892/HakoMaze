using System;
using HakoMaze.Constants;

namespace HakoMaze.ViewModels
{
    public static class MainWindowCommandFactory
    {
        public static MainWindowCommand New( string commandKey, MainWindowViewModel vm )
        {
            switch (commandKey) {
                case CommandKey.New: return new NewCommand( vm );
                case CommandKey.Open: return new OpenCommand( vm );
                case CommandKey.OpenSample: return new OpenSampleCommand( vm );
                case CommandKey.Save: return new SaveCommand( vm );
                case CommandKey.PutRedbox: return new PutRedboxCommand( vm );
                case CommandKey.PutYellowbox: return new PutYellowboxCommand( vm );
                case CommandKey.PutGreenboxes: return new PutGreenboxesCommand( vm );
                case CommandKey.PutWalls: return new PutWallsCommand( vm );
            }

            throw new NotImplementedException( $"commandKey \"{commandKey}\" は無効です" );
        }
    }
}
