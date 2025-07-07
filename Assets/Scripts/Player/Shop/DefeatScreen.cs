using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text conversionText;
    [SerializeField] public Money money;
    [SerializeField] private LoadManager load;
    void Start()
    {
        Time.timeScale = 0;
        ConversionScore();
    }

    void Update()
    {
        
    }

    void ConversionScore()
    {
        AudioManager.Instance.StopAllSounds();
        conversionText.text = $"money = score ({Money.score}) + money ({money.money}) = {money.money += Money.score}";
        Money.score = 0;
        load.Save();
        Money.combo.Clear();
        Debug.Log("Game Over");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
