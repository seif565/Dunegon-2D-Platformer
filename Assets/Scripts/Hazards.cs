using UnityEngine;

public class Hazards : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        FindObjectOfType<SessionManager>().TakeDamage(99);
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
