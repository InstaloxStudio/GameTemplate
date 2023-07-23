
public interface IAIState<T> where T : AIPawn<T>
{
    void Enter(T pawn);
    void Execute(T pawn);
    void Exit(T pawn);
}