
/// <summary>
/// Triggers enemies in a defined area to start chasing the player.
/// </summary>
public interface IEnemyTriggerArea
{
    float Are { get; }
    bool IsActive { get; }
    void Trigger();
}
