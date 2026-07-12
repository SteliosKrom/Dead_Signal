public interface IPatrolBot
{
    PatrolComponent PatrolComponent { get; }
    void MoveToPatrolPoint();
    void PlayWalkAnimation();
}