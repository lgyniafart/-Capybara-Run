using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    //Vector3 targetPos;
    float laneOffset; //расстояние между дорожками игрока
    public float laneChangeSpeed = 30; //скорость перемещения между дорожками

    Rigidbody rb;
    Vector3 targetVelocity;
    float pointStart; //стартовая точка при перемещении между дорожками
    float pointFinish; //финишная точка при перемещении между дорожками
    bool isMoving = false; //метод перемещения между дорожками
    Coroutine movingCoroutine; //обозначение карутины (карутины - функции, которые могут работать параллельно в течении какого-то времени)
    float lastVectorX;
    bool isJumping = false; //обозначение метода прыжка
    float jumpPower = 25; //сила прыжка
    float jumpGravity = -40; //сила гравитации при падении с рампы (не используется)
    float realGravity = -9.8f; //глобальная сила гравитации, действующая при прыжке

    void Start()
    {
        laneOffset = MapGenerator.instance.laneOffset; //запуск создания карт препятствий
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (Input.GetKeyDown(KeyCode.D) && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)
        {
            Jump();
        }
    } //метод осуществления движений по нажатым клавишам

    void Jump()
    {
        animator.applyRootMotion = false;
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopjumpCoroutine());
    } //метод прыжка игрока

    IEnumerator StopjumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    } //карутина для прыжка

    public void StartGame()
    {
        animator.SetTrigger("Run");
        RoadGenerator.instance.StartLevel();
    } //запуск игры/уровня

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        animator.applyRootMotion = true;
        animator.SetTrigger("IDLE");
        transform.position = startGamePosition;
        transform.rotation = startGameRotation;
        RoadGenerator.instance.ResetLevel();
    } //остановка/перезапуск игры/уровня

    void MoveHorizontal(float speed)
    {
        animator.applyRootMotion = false;
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving) { StopCoroutine(movingCoroutine); isMoving = false; }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    } //метод перемещения между дорожками

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while(Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            isMoving = true;
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;
    } //карутина для перемещения между дорожками

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            ResetGame();
        }
    } //физика, связанная с рампой (не используется) + столновение с препятствием

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    } //физика, связанная с рампой (не используется)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);
        }
    } //метод перемещения и столновения с объектами без проигрыша (т.к.рампы нет, то и этот метод бесполезен)

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")
        {
             if (rb.velocity.x == 0 && isJumping == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    } //физика, связанная с рампой (не используется)
} //скрипт контроля игрока
