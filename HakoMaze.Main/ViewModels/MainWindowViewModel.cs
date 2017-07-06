using FakeFrame;
using HakoMaze.Main.Commands;
using HakoMaze.Main.Constants;

namespace HakoMaze.Main.ViewModels
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

        RedboxTraceTreeViewModel treeViewModel;
        public RedboxTraceTreeViewModel TreeViewModel
        {
            get { return treeViewModel; }
            set {
                treeViewModel = value;
                RaiseProperyChanged( "TreeViewModel" );
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

        // ファイル
        public SwitchCommand NewCommand => New( CommandKey.New );
        public SwitchCommand OpenCommand => New( CommandKey.Open );
        public SwitchCommand OpenSampleCommand => New( CommandKey.OpenSample );
        public SwitchCommand SaveCommand => New( CommandKey.Save );
        public SwitchCommand PutRedboxCommand => New( CommandKey.PutRedbox );
        public SwitchCommand PutYellowboxCommand => New( CommandKey.PutYellowbox );
        public SwitchCommand PutGreenboxesCommand => New( CommandKey.PutGreenboxes );
        public SwitchCommand PutWallsCommand => New( CommandKey.PutWalls );
        // 探索
        public SwitchCommand ManualSearchCommand => New( CommandKey.ManualSearch );
        public SwitchCommand AutoSearchCommand => New( CommandKey.AutoSearch );

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
