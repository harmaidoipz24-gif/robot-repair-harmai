using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Налаштування механіки-твіста")]
    public bool isActive = true;
    public float dashMultiplier = 2.2f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 2.0f;
    public Color dashTint = new Color(0.5f, 0.9f, 1.0f, 1.0f);

    private PlayerController controller;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float baseSpeed;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (controller != null)
        {
            baseSpeed = controller.speed;
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        if (!isActive || controller == null) return;

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;

        // Збільшуємо швидкість та підсвічуємо спрайт для візуального зворотного зв'язку
        controller.speed = baseSpeed * dashMultiplier;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = dashTint;
        }

        yield return new WaitForSeconds(dashDuration);

        // Повертаємо стандартну швидкість і колір
        controller.speed = baseSpeed;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isDashing = false;

        // Очікування перезарядки (cooldown)
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}