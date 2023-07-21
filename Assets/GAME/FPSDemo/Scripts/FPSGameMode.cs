using UnityEngine;

public class FPSGameMode : GameMode
{

    public int KillsToWin = 3;
    public int CurrentKills = 0;
    public void OnPawnKilled(Pawn pawn)
    {
        CurrentKills++;
        if (CurrentKills >= KillsToWin)
        {
            // The player won, restart the game
            Debug.Log("You won!");
        }
        if (pawn is FPSDemoCharacter)
        {
            // The player died, restart the game
        }

    }
}
