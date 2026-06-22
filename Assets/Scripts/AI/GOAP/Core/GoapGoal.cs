
public class GoapGoal
{
    public string StateKey { get; set; }
    public bool DesiredValue { get; set; }

    public GoapGoal(string stateKey, bool desiredValue)
    {
        this.StateKey = stateKey;
        this.DesiredValue = desiredValue;
    }
}
