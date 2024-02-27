using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class AgentController : Agent
{
    public float speed = 10f;
    public float wallBound = 4.5f;
    private Vector3 initialP;
    private BallController ball;

    private void Start() {
        initialP = transform.position;
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
    }

    void Update()
    {
        // Получаем вход от агента
        float moveInput = GetInput();

        // Перемещаем ракетку
        MovePaddle(moveInput);
    }
    public override void OnEpisodeBegin()
    {
        // Установка агента в начальную позицию
        transform.position = initialP;

        // Установка других объектов сцены в начальные позиции
        ball.points = 0;

        // Установка новой позиции для мяча
        ball.LaunchBall();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(ball.transform.position);

    }
    // Получаем ввод от агента
    private float GetInput()
    {
        return Input.GetAxis("Vertical");
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Получение действий от агента
        float verticalAction = actionBuffers.ContinuousActions[0];

        // Применение действий к движению ракетки
        MovePaddle(verticalAction);

        // Некоторые дополнительные действия, если необходимо
        // ...

        // Проверка условий завершения эпизода, наград и т.д.
        CheckEpisodeConditions();
    }

    private void CheckEpisodeConditions()
    {
        if (ball.transform.position.x < -11f || ball.transform.position.x > 11f )
        {
            // Наградить или штрафовать, если необходимо
            if(ball.points <= 0)
            {
                AddReward(-1f);
            }
            else
            {
                AddReward(5f);
            }
            ball.speed = 5f;
            EndEpisode();
        }
    }

    // Перемещаем ракетку
    private void MovePaddle(float moveInput)
    {
        // Вычисляем новую позицию ракетки
        float newY = transform.position.y + moveInput * speed * Time.deltaTime;

        // Ограничиваем движение ракетки в пределах экрана
        newY = Mathf.Clamp(newY, -4.5f, 4.5f);

        // Устанавливаем новую позицию ракетки
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Вызывается при столкновении с мячом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Награда за столкновение с мячом
            AddReward(5f);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            float newY = Mathf.Clamp(transform.position.y, -wallBound, wallBound);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Получаем ввод от игрока или другим способом
        float verticalInput = Input.GetAxis("Vertical");

        // Заполняем буфер действий в соответствии с вводом
        actionsOut.ContinuousActions.Array[0] = verticalInput;
    }
}
