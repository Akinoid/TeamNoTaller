using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{
    [SerializeField] GameObject startMenuPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject controlsPanel;

    private void Start()
    {
        startMenuPanel = transform.Find("StartMenuPanel").gameObject;
        shopPanel = transform.Find("ShopPanel").gameObject;
        controlsPanel = transform.Find("ControlsPanel").gameObject;
        shopPanel.SetActive(false);
        controlsPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("Player");
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
    public void ReturnControlsButton()
    {
        startMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
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
}
