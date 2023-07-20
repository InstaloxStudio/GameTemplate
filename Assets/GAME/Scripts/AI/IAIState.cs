public interface IAIState
{
    void Enter(AIPawn pawn);
    void Execute(AIPawn pawn);
    void Exit(AIPawn pawn);
}