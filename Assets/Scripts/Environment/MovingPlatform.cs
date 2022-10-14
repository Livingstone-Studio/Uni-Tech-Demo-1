using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Components")]

    private Rigidbody2D rb;
    private Collider2D platformCollider;

    [Header("Patrol Settings")]

    [SerializeField] private Vector2[] patrolPoints;

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 origin;

    private int patrolIndex = 0;

    private Vector2 target;

    private bool updated = false;

    [SerializeField] private bool hor = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();

        origin = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        PatrolBetweenPoints(patrolPoints);
    }

    public void PatrolBetweenPoints(Vector2[] _points)
    {
        float distance = 0;

        if (hor)
        {
            distance = GetTarget(_points[patrolIndex]).x - transform.position.x;
        }
        else
        {
            distance = GetTarget(_points[patrolIndex]).y - transform.position.y;
        }

        distance = Mathf.Sqrt(distance * distance);

        if (distance < 0.1f && !updated)
        {
            updated = true;
            patrolIndex++;

            if (patrolIndex >= _points.Length)
            {
                patrolIndex = 0;
            }
        }
        else
        {
            updated = false;
            AxisMove(GetTarget(_points[patrolIndex]));
        }
    }
    public void AxisMove(Vector2 _newPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, _newPos, Time.deltaTime * moveSpeed);
    }

    private Vector2 GetTarget(Vector2 point)
    {
        return origin + point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
