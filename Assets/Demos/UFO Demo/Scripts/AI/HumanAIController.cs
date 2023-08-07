using UnityEngine;

public class HumanAIController : AIController<HumanPawn>
{
    public HumanAIController(HumanPawn pawn) : base(pawn)
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

    }
}