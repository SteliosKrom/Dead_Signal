using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static bool EnterGameplay;

    #region COROUTINES
    private float loadingDelay;
    #endregion

    #region SERVICES
    private UIManager uiManager;
    #endregion

    private void Start()
    {
        uiManager = ServiceManager.GetService<UIManager>();

        loadingDelay = Random.Range(1, 5);

        StartCoroutine(LoadingTimeCoroutine());
    }

    private IEnumerator LoadingTimeCoroutine()
    {
        yield return new WaitForSeconds(loadingDelay);
        EnterGameplay = true;
        SceneManager.LoadScene("Main");
    }
}
