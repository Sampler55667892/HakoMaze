using System.Windows;

namespace HakoMaze.ViewModels
{
    public class PutRedboxCommand : PutBoxCommand
    {
        public PutRedboxCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        public override void OnAct()
        {
            base.OnAct();
            if (StopsAct)
                return;

            //...
        }
    }
}
