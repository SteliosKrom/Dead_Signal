using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event Action OnGameplayStarted;
    public event Action OnGameOver;

    public void RaiseGameplayStarted() => OnGameplayStarted?.Invoke();
    public void RaiseGameOver() => OnGameOver?.Invoke();
}