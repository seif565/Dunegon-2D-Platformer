using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{    
    [SerializeField] Image healthImage;
    [SerializeField] [Range(0,10)] int startingHealth;
    int currentHealth;
    private void Awake()
    {
        if (FindObjectsOfType<SessionManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        currentHealth = startingHealth;
        UpdateHealth();        
    }

    private void UpdateHealth()
    {
        healthImage.rectTransform.sizeDelta = new Vector2(currentHealth == 0 ? 0 : 100 * currentHealth, 100);
        healthImage.rectTransform.localPosition = new Vector2
            (healthImage.rectTransform.localPosition.x + (50 * currentHealth), healthImage.rectTransform.localPosition.y);
    }

    void TakeDamage()
    {
        currentHealth--;
        UpdateHealth();
        if (currentHealth == 0) Die();
    }
    void Die()
    {
        Debug.Log("Game Over!!");
    }
}
