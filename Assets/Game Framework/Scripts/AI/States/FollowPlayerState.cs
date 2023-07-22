public class FollowPlayerState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        ((FriendlyAIPawn)pawn).FollowPlayer();
    }

    public void Execute(AIPawn pawn)
    {
    }

    public void Exit(AIPawn pawn)
    {
    }
}
