using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class GameBall : MonoBehaviour
{
    public event Action OnBallDestroyed;

    public void OnDestroy()
    {
        if (OnBallDestroyed != null)
        {
            OnBallDestroyed();
        }
    }
}