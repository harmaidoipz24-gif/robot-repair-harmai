using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHealth = 5;
    public GameObject projectilePrefab;

    // Властивість читання HP для HealthCollectible
    public int health { get { return currentHealth; } }

    private Rigidbody2D rigidbody2d;
    private Vector2 move;
    private Vector2 lookDirection = new Vector2(1, 0);
    private int currentHealth;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position += move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (UIHandler.instance != null)
        {
            UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        }

        if (currentHealth <= 0)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    private void Launch()
    {
        if (projectilePrefab == null) return;

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Launch(lookDirection, 300);
        }
    }
}