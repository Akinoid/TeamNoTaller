using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private ArrowController arrowController;
    private void Start()
    {
        arrowController.wantToUnlock = false;
        pausePanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartPause();
        }
    }
    void StartPause()
    {
        arrowController.wantToUnlock = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void Resume()
    {
        arrowController.wantToUnlock = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
