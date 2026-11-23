using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public int maxHealth = 5;
    public float timeInvincible = 1.5f;
    public GameObject projectilePrefab;

    [Header("Зворотний зв'язок (Полішинг)")]
    public AudioClip hitSound;
    public AudioClip healSound;
    public AudioClip throwSound;
    public Color damageFlashColor = new Color(1f, 0.35f, 0.35f, 1f);
    public Color healFlashColor = new Color(0.4f, 1f, 0.4f, 1f);
    public float flashDuration = 0.15f;

    public int health { get { return currentHealth; } }

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Vector2 move;
    private Vector2 lookDirection = new Vector2(1, 0);
    private int currentHealth;
    private bool isInvincible;
    private float damageCooldown;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
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
        if (amount < 0)
        {
            if (isInvincible) return; // Захист: один звук і один удар на подію
            isInvincible = true;
            damageCooldown = timeInvincible;

            PlaySound(hitSound);
            StartCoroutine(FlashSprite(damageFlashColor));
        }
        else if (amount > 0)
        {
            PlaySound(healSound);
            StartCoroutine(FlashSprite(healFlashColor));
        }

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
            PlaySound(throwSound);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator FlashSprite(Color flashColor)
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }
}