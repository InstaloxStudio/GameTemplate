using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSGameMode : GameMode
{

    public int KillsToWin = 3;
    public int CurrentKills = 0;
    bool isGameOver = false;
    public void OnPawnKilled(Pawn pawn)
    {
        if (isGameOver)
            return;


        if (pawn is FPSDemoCharacter)
        {
            isGameOver = true;
            Debug.Log("You died!");
            return;
        }

        CurrentKills++;
        if (CurrentKills >= KillsToWin)
        {
            // The player won, restart the game
            Debug.Log("You won!");
        }


    }
}
