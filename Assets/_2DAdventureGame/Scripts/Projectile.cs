using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Снаряд торкнувся: " + other.name); // Лог для перевірки
        if (other.GetComponent<PlayerController>() != null)
        {
            return;
        }

        // Перевіряємо влучання у ворога
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
            Destroy(gameObject); // Знищуємо снаряд при влучанні у ворога
        }
    

    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}