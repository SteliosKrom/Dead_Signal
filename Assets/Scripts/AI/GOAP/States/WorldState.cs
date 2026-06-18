using System.Collections.Generic;

public class WorldState
{
    private Dictionary<string, bool> states = new Dictionary<string, bool>();

    public void SetState(string key, bool value)
    {
        states[key] = value;
    }

    public bool GetState(string key)
    {
        if (states.TryGetValue(key, out bool value))
        {
            return value;
        }
        return false;
    }
}
