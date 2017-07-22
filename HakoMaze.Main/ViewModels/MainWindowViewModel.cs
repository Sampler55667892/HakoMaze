using FakeFrame;
using HakoMaze.Main.Commands;
using HakoMaze.Main.Constants;

namespace HakoMaze.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
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
        public RelayCommand NewCommand => New( CommandKey.New );
        public RelayCommand OpenCommand => New( CommandKey.Open );
        public RelayCommand OpenSampleCommand => New( CommandKey.OpenSample );
        public RelayCommand SaveCommand => New( CommandKey.Save );
        // 配置
        public RelayCommand PutRedboxCommand => New( CommandKey.PutRedbox );
        public RelayCommand PutYellowboxCommand => New( CommandKey.PutYellowbox );
        public RelayCommand PutGreenboxesCommand => New( CommandKey.PutGreenboxes );
        public RelayCommand PutWallsCommand => New( CommandKey.PutWalls );
        public RelayCommand PutGoalCommand => New( CommandKey.PutGoal );
        // 探索
        public RelayCommand ManualSearchCommand => New( CommandKey.ManualSearch );
        public RelayCommand AutoSearchCommand => New( CommandKey.AutoSearch );

        // View側で設定 (View要素にアクセス)
        public RelayCommand CloseCommand { get; set; }

        RelayCommand New( string key ) =>
            new RelayCommand( x => CommandQueue.Instance.Enqueue( MainWindowCommandFactory.New( key, this ) ) );
    }
}
