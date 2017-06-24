namespace HakoMaze.ViewModels
{
    public class PutRedboxCommand : PutBoxCommand
    {
        public PutRedboxCommand( MainWindowViewModel vm ) : base( vm )
        {
        }

        protected override bool ModifyContentData( (int x, int y) hitCellPosition )
        {
            // 黄箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.YellowboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.YellowboxPosition.Value.Equals( hitCellPosition ))
                return false;

            // 緑箱が既にあれば何もしない
            if (CanvasViewModel.MazeContentData.ExistsGreenboxPosition( hitCellPosition ))
                return false;

            // 同じ位置の赤箱をクリック → 赤箱を削除
            // 赤箱がない or 異なる位置に存在 → 赤箱の位置を更新
            if (CanvasViewModel.MazeContentData.RedboxPosition.HasValue &&
                CanvasViewModel.MazeContentData.RedboxPosition.Value.Equals( hitCellPosition )) {
                CanvasViewModel.MazeContentData.RedboxPosition = null;
                AddHistoryMessage( $"赤箱の削除：{hitCellPosition}" );
            } else {
                CanvasViewModel.MazeContentData.RedboxPosition = hitCellPosition;
                AddHistoryMessage( $"赤箱の追加/更新：{hitCellPosition}" );

            }

            return true;
        }
    }
}
