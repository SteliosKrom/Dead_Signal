using System.Collections.Generic;

public class GoapAction
{
    private string name;

    private Dictionary<string, bool> preconditions = new Dictionary<string, bool>();
    private Dictionary<string, bool> effects = new Dictionary<string, bool>();

    #region PROPERTIES
    public string Name { get; set; }
    public Dictionary<string, bool> Preconditions => preconditions;
    public Dictionary<string, bool> Effects => effects;
    #endregion

    public bool CanExecute(WorldState world)
    {
        foreach (KeyValuePair<string, bool> condition in preconditions)
        {
            if (world.GetState(condition.Key) != condition.Value)
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyEffects(WorldState world)
    {
        foreach (KeyValuePair<string, bool> effect in effects)
        {
            world.SetState(effect.Key, effect.Value);
        }
    }
}
