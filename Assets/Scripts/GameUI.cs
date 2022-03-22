using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;

    void Start()
    {
        UpdateUI(FindObjectOfType<SessionManager>().GetHealth());
    }

    public void UpdateUI(int health)
    {                
        if(health > 0)
        {
            livesText.SetText(health.ToString());
        }
        else
        {
            livesText.SetText("0");
        }
    }

    
    
    
}
