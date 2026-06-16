using UnityEngine;

public class DoorDetectable : MonoBehaviour
{
    [SerializeField] private int zMaxHits;
    [SerializeField] private int currentHit;

    public void TakeHit()
    {
        currentHit++;

        if (currentHit >= zMaxHits)
        {
            gameObject.SetActive(false);
        }
    }
}
