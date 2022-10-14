using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Collider2D platformCollider;

    private Moveable moveable;

    private void Update()
    {
        if (!moveable) return;

        if (moveable.moveState != MoveState.LADDER && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            moveable.GetOnLadder();

            if (platformCollider)
            {
                platformCollider.enabled = false;
            }
        }

        if (moveable.moveState == MoveState.LADDER)
        {
            Vector3 ladderPos = new Vector3(transform.position.x, moveable.transform.position.y);

            moveable.transform.position += (ladderPos - moveable.transform.position) * Time.deltaTime * 20;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveable = collision.GetComponent<Moveable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveable = collision.GetComponent<Moveable>();

            if (moveable)
            {
                moveable.GetOffLadder();

                if (platformCollider)
                {
                    platformCollider.enabled = true;
                }

                moveable = null;
            }
        }
    }
}
