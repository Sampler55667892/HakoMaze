namespace HakoMaze.Common
{
    // ムーア機械としよう
    public abstract class StateMachine<T>
    {
        public int State { get; set; }

        // 入力があると状態を遷移し，出力を返す
        public abstract T OnAct( T input );
    }
}
