using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    private Rigidbody2D rigidbody2d;
    private float timer;
    private int direction = 1;
    private bool broken = true;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    private void Update()
    {
        if (!broken) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        if (!broken) return;

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + Time.fixedDeltaTime * speed * direction;
        }
        else
        {
            position.x = position.x + Time.fixedDeltaTime * speed * direction;
        }

        rigidbody2d.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        if (rigidbody2d != null)
        {
            rigidbody2d.simulated = false;
        }

        Debug.Log("--- МЕТОД FIX ВИКЛИКАНО! ---");

        if (GameManager.instance != null)
        {
            GameManager.instance.RobotFixed();
        }
    }
}