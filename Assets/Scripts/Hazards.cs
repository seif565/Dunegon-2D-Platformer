using UnityEngine;

public class Hazards : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<SessionManager>().TakeDamage(99);
    }
}
