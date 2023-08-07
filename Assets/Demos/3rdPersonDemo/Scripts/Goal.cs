using UnityEngine;

public class Goal : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        var gameBall = other.GetComponent<GameBall>();
        if (gameBall != null)
        {
            Debug.Log("Goal!");
        }
    }
}