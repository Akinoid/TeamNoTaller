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
        AudioSettingsManager.Instance.SetMute(true);
        arrowController.wantToUnlock = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void BackToMenu()
    {
        AudioManager.Instance.StopAllSounds();
        AudioSettingsManager.Instance.SetMute(false);
        SceneManager.LoadScene("StartMenu");
    }
    public void Resume()
    {
        AudioSettingsManager.Instance.SetMute(false);
        arrowController.wantToUnlock = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
