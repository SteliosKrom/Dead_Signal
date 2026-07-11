using System;

public interface IIdle
{
    float IdleTimer { get; set; }
    float IdleTimeInterval { get; }
    void OnIdleFinished();
    void PlayIdleAnimation();
}
