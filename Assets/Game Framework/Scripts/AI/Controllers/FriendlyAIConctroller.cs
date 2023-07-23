using UnityEngine;

public class FriendlyAIController : AIController<FriendlyAIPawn>
{
    public FriendlyAIController(FriendlyAIPawn pawn) : base(pawn)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        //  Pawn.ChangeState(new IdleState());
    }

    public override void Update()
    {
        base.Update();
        if (pawn.IsPlayerTooFar())
        {
            //pawn.ChangeState(new FollowPlayerState());
        }
        else
        {
            // pawn.ChangeState(new IdleState());
        }
    }
}
