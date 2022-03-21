using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{    
    [SerializeField] Image healthImage;
    [SerializeField] Canvas gameCanvas;
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
        DontDestroyOnLoad(gameCanvas);                
    }
    void Start()
    {
        currentHealth = startingHealth;
        UpdateHealth();        
    }

    private void UpdateHealth()
    {
        healthImage.rectTransform.sizeDelta = new Vector2(currentHealth <= 0 ? 0 : 100 * currentHealth, 100);
        healthImage.rectTransform.localPosition = new Vector2
            (healthImage.rectTransform.localPosition.x + (50 * currentHealth), healthImage.rectTransform.localPosition.y);
    }

    public void TakeDamage(int damageAmount = 1)
    {
        currentHealth -= damageAmount;
        UpdateHealth();
        if (currentHealth <= 0) {
            FindObjectOfType<Hero>().Die();
            Destroy(FindObjectOfType<Hero>().GetComponent<Hero>());
            Invoke("ResetLevel", 3f);
        }
    }    
    void ResetLevel()
    {
        currentHealth = startingHealth;
        UpdateHealth();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
