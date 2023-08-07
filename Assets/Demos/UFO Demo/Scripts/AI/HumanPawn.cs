using UnityEngine;
using System.Collections;


public class HumanPawn : AIPawn<HumanPawn>
{
    protected override void Start()
    {
        AIController = new HumanAIController(this);
        AIController.Initialize();
    }
}