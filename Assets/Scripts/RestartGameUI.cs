using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class RestartGameUI : MonoBehaviour
{
    public GameObject restartPanel;
    public Button newGameButton;

    void Start()
    {
        restartPanel.SetActive(false);
        newGameButton.onClick.AddListener(RestartGame);
    }

    public void ShowRestartPanel()
    {
        restartPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
