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
    }
    public void PlayButton()
    {
        if(tutorial == 0)
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
        startMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void ControlsButton()
    {
        startMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OptionsButton()
    {
        startMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void ReturnControlsButton()
    {
        startMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    public void ReturnOptionsButton()
    {
        startMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void ReturnShopButton()
    {
        startMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void ResetTutorialButton()
    {
        tutorial = 0;
        PlayerPrefs.SetInt("Tutorial", 0);
    }
}
