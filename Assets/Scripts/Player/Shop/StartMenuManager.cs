using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{
    [SerializeField] GameObject startMenuPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject optionsPanel;
    public static int tutorial;
    private void Start()
    {
        startMenuPanel = transform.Find("StartMenuPanel").gameObject;
        shopPanel = transform.Find("ShopPanel").gameObject;
        controlsPanel = transform.Find("ControlsPanel").gameObject;
        optionsPanel = transform.Find("OptionsPanel").gameObject;
        shopPanel.SetActive(false);
        controlsPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        tutorial = PlayerPrefs.GetInt("Tutorial", 0);
        AudioManager.Instance.Play("Menu Music");
    }
    public void PlayButton()
    {
        AudioManager.Instance.Play("UI Button");
        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.Play("Space Music");
        if (tutorial == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Player");
        }
    }

    public void ShopButton()
    {
        AudioManager.Instance.Play("UI Button");
        AudioManager.Instance.Stop("Menu Music");
        AudioManager.Instance.Play("Store Music");
        startMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void ControlsButton()
    {
        AudioManager.Instance.Play("UI Button");
        startMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OptionsButton()
    {
        AudioManager.Instance.Play("UI Button");
        startMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void ReturnControlsButton()
    {
        AudioManager.Instance.Play("UI Button");
        startMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    public void ReturnOptionsButton()
    {
        AudioManager.Instance.Play("UI Button");
        startMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void ReturnShopButton()
    {
        AudioManager.Instance.Play("UI Button");
        
        startMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
        AudioManager.Instance.Stop("Store Music");
        AudioManager.Instance.Play("Menu Music");
    }
    public void ExitButton()
    {
        AudioManager.Instance.Play("UI Button");
        Application.Quit();
    }
    public void ResetTutorialButton()
    {
        AudioManager.Instance.Play("UI Button");
        tutorial = 0;
        PlayerPrefs.SetInt("Tutorial", 0);
        SceneManager.LoadScene("Tutorial");
    }
}
