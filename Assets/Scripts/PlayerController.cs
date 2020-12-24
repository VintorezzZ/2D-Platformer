using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] LayerMask groundMask;
    public bool isGrounded = false;
    public bool gameOver = false;
    private float jumpRatio = 0;
    private bool canJump = true;
    private float timer = 0;
    public static event Action onGameOver;
    public static event Action onWin;
    //private Action action;
    void Start()
    {
        onGameOver += RespawnPlayer;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        CheckForGround();
        CheckForGameOver();
        
        #region jump

        if (Input.GetKey(KeyCode.Space) && canJump && isGrounded)
        {
            jumpRatio += Time.deltaTime;
            if (jumpRatio > 0.1f)
            {
                //print(jumpRatio);
                Jump();
                jumpRatio -= jumpRatio;
                canJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //print(jumpRatio);
            Jump();
            jumpRatio -= jumpRatio;
            canJump = true;
        }

        #endregion
    }
    private void FixedUpdate()
    {
        MoveForward();
    }
    private void MoveForward()
    {
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce * jumpRatio, ForceMode2D.Impulse);
    }
    private void CheckForGround()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector3.down, 0.6f, groundMask);
    }

    private void CheckForGameOver()
    {
        if (!isGrounded)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        if (timer > 2f)
        {
            timer = 0;
            onGameOver?.Invoke();
        }
    }

    private void RespawnPlayer()
    {
        //gameOver = true;
        transform.position = new Vector3(-2.5f, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            onWin?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }
            
        if (other.gameObject.CompareTag("Obstacle"))
            RespawnPlayer();
    }

    private void OnDisable()
    {
        onGameOver -= RespawnPlayer;
    }
}
