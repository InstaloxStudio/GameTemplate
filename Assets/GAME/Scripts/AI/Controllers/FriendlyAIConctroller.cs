using UnityEngine;

public class FriendlyAIController : AIController
{
    public FriendlyAIController(AIPawn pawn) : base(pawn)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Pawn.ChangeState(new IdleState());
    }

    public override void Update()
    {
        base.Update();
        FriendlyAIPawn friendlyPawn = (FriendlyAIPawn)Pawn;
        if (friendlyPawn.IsPlayerTooFar())
        {
            friendlyPawn.ChangeState(new FollowPlayerState());
        }
        else
        {
            friendlyPawn.ChangeState(new IdleState());
        }
    }
}
