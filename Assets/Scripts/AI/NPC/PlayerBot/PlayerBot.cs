using UnityEngine;

public class PlayerBot : MonoBehaviour
{
    [SerializeField] private BotRole currentRole;

    #region PROPERTIES
    public BotRole CurrentRole { get => currentRole; set => currentRole = value; }
    #endregion
    private void Start()
    {
        currentRole = BotRole.None;
    }
}