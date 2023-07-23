public class FollowPlayerState : IAIState<FriendlyAIPawn>
{
    public void Enter(FriendlyAIPawn pawn)
    {
        pawn.FollowPlayer();
    }

    public void Execute(FriendlyAIPawn pawn)
    {
    }

    public void Exit(FriendlyAIPawn pawn)
    {
    }
}
