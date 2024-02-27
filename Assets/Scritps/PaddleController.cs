using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 5f;
    public float wallBound = 4.5f;
    void Update()
    {
        // Получаем вход от пользователя
        float moveInput = Input.GetAxis("Vertical");

        // Перемещаем ракетку
        MovePaddle(moveInput);
    }

    void MovePaddle(float moveInput)
    {
        // Вычисляем новую позицию ракетки
        float newY = transform.position.y + moveInput * speed * Time.deltaTime;

        // Ограничиваем движение ракетки в пределах экрана
        newY = Mathf.Clamp(newY, -4.5f, 4.5f);

        // Устанавливаем новую позицию ракетки
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Ограничиваем движение ракетки, если она сталкивается со стеной
            float newY = Mathf.Clamp(transform.position.y, -wallBound, wallBound);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
