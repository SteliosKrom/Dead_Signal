using UnityEngine;

public class PlayerBot : MonoBehaviour
{
    [SerializeField] private BotRole currentRole;

    [SerializeField] private int currentAmmo;

    #region PROPERTIES
    public int CurrentAmmo
    {
        get => currentAmmo;
        set
        {
            if (value < 0)
            {
                currentAmmo = 0;
            }
            else
            {
                currentAmmo = value;
            }
        }
    }
    public BotRole CurrentRole { get => currentRole; set => currentRole = value; }
    #endregion
    private void Start()
    {
        currentRole = BotRole.None;
    }
}