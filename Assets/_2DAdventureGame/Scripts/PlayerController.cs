using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
    public GameObject projectilePrefab;
    public InputAction launchAction;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;

    Rigidbody2D rigidbody2d;
    Vector2 move;

    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        launchAction.Enable();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
       // Debug.Log(move);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
   
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (launchAction.WasPressedThisFrame())
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        // Вказуємо напрямок пострілу (використовуємо поточний напрямок погляду lookDirection)
        projectile.Launch(lookDirection, 300);

        // Тригер анімації кидка
        animator.SetTrigger("Launch");
    }
}