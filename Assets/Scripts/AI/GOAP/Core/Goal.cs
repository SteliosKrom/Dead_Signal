
public class Goal
{
    public string StateKey { get; set; }
    public bool DesiredValue { get; set; }

    public Goal(string stateKey, bool desiredValue)
    {
        this.StateKey = stateKey;
        this.DesiredValue = desiredValue;
    }
}
