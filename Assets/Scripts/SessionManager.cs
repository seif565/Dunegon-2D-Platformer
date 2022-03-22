using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    GameUI gameUI;
    [SerializeField] [Range(0,10)] int startingHealth;
    int currentHealth;    
    void Awake()
    {
        gameUI = FindObjectOfType<GameUI>();
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
        FindObjectOfType<GameUI>().UpdateUI(currentHealth);     
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    

    public void TakeDamage(int damageAmount = 1)
    {
        if(gameUI == null)
        {
            gameUI = FindObjectOfType<GameUI>();
        }
        currentHealth -= damageAmount;
        gameUI.UpdateUI(currentHealth);
        if (currentHealth <= 0) {
            FindObjectOfType<Hero>().Die();
            Destroy(FindObjectOfType<Hero>().GetComponent<Hero>());
            Invoke("ResetLevel", 3f);
        }
    }    
    void ResetLevel()
    {
        currentHealth = startingHealth;
        gameUI.UpdateUI(startingHealth);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
