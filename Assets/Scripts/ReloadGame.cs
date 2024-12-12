using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadGame : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnPlayGame);
    }

    void OnPlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
