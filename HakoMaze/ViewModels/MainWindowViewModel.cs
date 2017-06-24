using Common.WpfCommands;
using HakoMaze.Common;
using HakoMaze.Constants;
using HakoMaze.Models;

namespace HakoMaze.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        CommandScheduler commandScheduler;
        MazeFrameData mazeFrameData = new MazeFrameData();
        MazeContentData mazeContentData = new MazeContentData();

        public MazeFrameData MazeFrameData => mazeFrameData;
        public MazeContentData MazeContentData => mazeContentData;

        MazeFrameViewModel canvasViewModel;
        public MazeFrameViewModel CanvasViewModel
        {
            get { return canvasViewModel; }
            set {
                canvasViewModel = value;
                RaiseProperyChanged( "CanvasViewModel" );
            }
        }

        string tempMessageLineText;
        public string TempMessageLineText
        {
            get { return tempMessageLineText; }
            set {
                tempMessageLineText = value;
                RaiseProperyChanged( "TempMessageLineText" );
            }
        }

        string historyMessageAreaText;
        public string HistoryMessageAreaText
        {
            get { return historyMessageAreaText; }
            set {
                historyMessageAreaText = value;
                RaiseProperyChanged( "HistoryMessageAreaText" );
            }
        }

        public SwitchCommand NewCommand => New( CommandKey.New );
        public SwitchCommand OpenCommand => New( CommandKey.Open );
        public SwitchCommand SaveCommand => New( CommandKey.Save );
        public SwitchCommand PutRedboxCommand => New( CommandKey.PutRedbox );
        public SwitchCommand PutYellowboxCommand => New( CommandKey.PutYellowbox );
        public SwitchCommand PutGreenboxesCommand => New( CommandKey.PutGreenboxes );
        public SwitchCommand PutWallsCommand => New( CommandKey.PutWalls );

        // View側で設定 (View要素にアクセス)
        public RelayCommand CloseCommand { get; set; }

        public MainWindowViewModel( CommandScheduler commandScheduler )
        {
            this.commandScheduler = commandScheduler;
        }

        SwitchCommand New( string key ) =>
            new SwitchCommand( commandScheduler, MainWindowCommandFactory.New( key, this ) );
    }
}
