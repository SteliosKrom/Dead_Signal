using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event Action OnGameplayStarted;

    public void RaiseGameplayStarted() => OnGameplayStarted?.Invoke();
}