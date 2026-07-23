using UnityEngine;
using UnityEngine.SceneManagement;

public class PCGSceneManager : MonoBehaviour
{
    public void GetBackToMainGame()
    {
        SceneManager.LoadScene("Main");
    }
}
