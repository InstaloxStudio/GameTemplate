using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text healthText;
    public Text KillsText;

    public PlayerData playerData;

    void Update()
    {
        healthText.text = "Health: " + playerData.health;
        KillsText.text = "Kills: " + playerData.kills;
    }

}