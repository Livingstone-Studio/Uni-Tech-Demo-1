using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class InputHandler : MonoBehaviour
{
    [Header("Components")]

    private Moveable moveable;

    [Header("Input Settings")]

    private float inputSensitivity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        moveable = GetComponent<Moveable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveable.moveState == MoveState.WALK && !CameraController.Instance.moving)
        {
            GetMovementInput();
        }
        else if (moveable.moveState == MoveState.LADDER && !CameraController.Instance.moving)
        {
            GetLadderInput();
        }
    }

    private void GetMovementInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveable.animator.speed == 0 && !CameraController.Instance.moving)
        {
            moveable.animator.speed = 1f;
        }

        if (moveInput > inputSensitivity)
        {
            transform.localScale = new Vector3(1, 1, 1);

            moveable.Move(Vector2.right);

            moveable.animator.SetBool("isRunning", true);
        }
        else if (moveInput < -inputSensitivity)
        {
            transform.localScale = new Vector3(-1, 1, 1);

            moveable.Move(Vector2.left);

            moveable.animator.SetBool("isRunning", true);
        }
        else
        {
            moveable.Move(Vector2.zero);

            moveable.animator.SetBool("isRunning", false);
        }

        if (Input.GetButton("Jump") && (moveable.jumping || !moveable.isFalling))
        {
            moveable.Jump();
        }
        else if (Input.GetButtonUp("Jump") && (moveable.jumping || !moveable.isFalling))
        {
            moveable.jumpLimit = true;
            moveable.jumping = false;
        }
    }

    private void GetLadderInput()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.y > inputSensitivity)
        {
            transform.localScale = new Vector3(1, 1, 1);

            moveable.LadderMove(Vector2.up);

            moveable.animator.speed = 1f;
        }
        else if (moveInput.y < -inputSensitivity)
        {
            transform.localScale = new Vector3(-1, 1, 1);

            moveable.LadderMove(Vector2.down);

            moveable.animator.speed = 1f;
        }
        else
        {
            moveable.LadderMove(Vector2.zero);

            moveable.animator.speed = 0f;
        }

        if (moveInput.x > inputSensitivity || moveInput.x < -inputSensitivity)
        {
            moveable.GetOffLadder();
        }
    }
}