using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementOptions
{
    Left, Right, Up, Down, Null
}
public enum Direction
{
    Left, Right, Up, Down
}

public class Movement : MonoBehaviour
{
    public int speed = 50;
    public Vector2 direction;
    public bool controlEnabled = false;
    public MovementOptions moveInput = MovementOptions.Null;
    public Direction dir = Direction.Left;
    public Rigidbody2D playerRB;
    public bool blocked;

    void Start()
    {
        //Animacion de spawn
        controlEnabled = true;
        playerRB = GetComponent<Rigidbody2D>();
        direction = new Vector2(-1, 0);
        dir = Direction.Left;
    }


    void Update()
    {
        // Input
        if (controlEnabled)
        {
            if (Input.GetButtonDown("Left") && dir != Direction.Left)
            {
                transform.eulerAngles= new Vector3(0,0,90);
                moveInput = MovementOptions.Left;
            }
            if (Input.GetButtonDown("Right") && dir != Direction.Right)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
                moveInput = MovementOptions.Right;
            }
            if (Input.GetButtonDown("Up") && dir != Direction.Up)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveInput = MovementOptions.Up;
            }
            if (Input.GetButtonDown("Down") && dir != Direction.Down)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
                moveInput = MovementOptions.Down;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (moveInput == MovementOptions.Down && !blocked)
        {
            direction = new Vector2(0, -1);
            dir = Direction.Down;
        }

        else if (moveInput == MovementOptions.Up && !blocked)
        {
            direction = new Vector2(0, 1);
            dir = Direction.Up;
        }

        else if (moveInput == MovementOptions.Right && !blocked)
        {
            direction = new Vector2(1, 0);
            dir = Direction.Right;
        }

        else if (moveInput == MovementOptions.Left && !blocked)
        {
            direction = new Vector2(-1, 0);
            dir = Direction.Left;
        }

        playerRB.velocity = direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Block")
        {
            blocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Block")
        {
            blocked = false;
        }
    }
}
