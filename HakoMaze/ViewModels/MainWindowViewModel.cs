using Common.WpfCommands;
using HakoMaze.Common;
using HakoMaze.Constants;

namespace HakoMaze.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public CommandScheduler CommandScheduler { get; private set; }

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
        public SwitchCommand OpenSampleCommand => New( CommandKey.OpenSample );
        public SwitchCommand SaveCommand => New( CommandKey.Save );
        public SwitchCommand PutRedboxCommand => New( CommandKey.PutRedbox );
        public SwitchCommand PutYellowboxCommand => New( CommandKey.PutYellowbox );
        public SwitchCommand PutGreenboxesCommand => New( CommandKey.PutGreenboxes );
        public SwitchCommand PutWallsCommand => New( CommandKey.PutWalls );

        // View側で設定 (View要素にアクセス)
        public RelayCommand CloseCommand { get; set; }

        public MainWindowViewModel( CommandScheduler commandScheduler )
        {
            this.CommandScheduler = commandScheduler;
        }

        SwitchCommand New( string key ) =>
            new SwitchCommand( CommandScheduler, MainWindowCommandFactory.New( key, this ) );
    }
}
