using UnityEngine;

public class BallController : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    public Vector2 direction;
    public float speed = 5f;
    public float coefficientSpeed = 1.1f;
    public int points = 0;
    private float stuckTime = 0f; // Время, которое мяч находится в застревшем состоянии
    public float maxStuckTime = 3f; // Максимальное время для считывания как застревшего
    private Vector3 lastBallPosition;
    private float stuckThreshold = 0.01f;

    void Start()
    {
        // Запускаем мяч в случайном направлении
        LaunchBall();
    }

    void Update()
    {
        // Перемещаем мяч вперед
        MoveBall();
    }

    // Запускаем мяч в случайном направлении
    public void LaunchBall()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        transform.position = Vector3.zero;
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    // Перемещаем мяч
    private void MoveBall()
    {
        rigidbody.velocity = direction.normalized * speed;
        // Проверка, застрял ли мяч
        if (IsBallStuck())
        {
            stuckTime += Time.deltaTime;

            // Если мяч застрял слишком долго, перебросьте его
            if (stuckTime > maxStuckTime)
            {
                stuckTime = 0f;
                LaunchBall();
            }
        }
        else
        {
            // Сбросить время, если мяч двигается
            stuckTime = 0f;
        }
    }

    private bool IsBallStuck()
    {
        // Проверка изменения позиции мяча
        if (Vector3.Distance(lastBallPosition, transform.position) < stuckThreshold)
        {
            return true;
        }

        lastBallPosition = transform.position;
        return false;
    }

    // Вызывается при столкновении с ракеткой
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction.x = -direction.x;
            direction.y = direction.y+Random.Range(-0.1f, 0.1f);
            speed *= coefficientSpeed;
            points += 1;
            Debug.Log("количество очков: " + points);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y = -direction.y;
            direction.x = direction.x+Random.Range(-0.1f, 0.1f);
        }
    }
}
