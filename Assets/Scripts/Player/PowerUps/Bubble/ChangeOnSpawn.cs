using UnityEngine;

public class ChangeOnSpawn : MonoBehaviour
{
    [SerializeField] private GameObject bubble, electricBubble;
    void Start()
    {
        if (Bubble.haveElectricBuff)
        {
            electricBubble.SetActive(true);
            bubble.SetActive(false);
        }
        if (!Bubble.haveElectricBuff)
        {
            bubble.SetActive(true);
            electricBubble.SetActive(false);
        }
    }

}
