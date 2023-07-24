using UnityEngine;

public class ThirdPersonDemoGameMode : GameMode
{
    public int goalsToWin = 3;
    public int currentGoals = 0;
    bool isGameOver = false;

    public GameObject ballPrefab;

    public override void Awake()
    {
        base.Awake();
        // Spawn the ball
        SpawnBall();
    }

    public void SpawnBall()
    {
        var ball = Instantiate(ballPrefab);
        ball.GetComponent<GameBall>().OnBallDestroyed += OnBallDestroyed;
    }

    public void OnBallDestroyed()
    {
        if (isGameOver)
            return;

        currentGoals++;
        if (currentGoals >= goalsToWin)
        {
            // The player won, restart the game
            Debug.Log("You won!");
            isGameOver = true;
            return;
        }

        // Spawn the ball
        SpawnBall();
    }

}