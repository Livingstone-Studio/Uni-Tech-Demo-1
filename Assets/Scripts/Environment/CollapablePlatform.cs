using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CollapablePlatform : MonoBehaviour
{

    [Header("Components")]

    private Rigidbody2D rb;
    [SerializeField] private Collider2D platformCollider;

    [Header("Settings")]

    [SerializeField] private float dropTimer = 1f;
    [SerializeField] private float respawnTimer = 5f;

    private Vector2 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();

        originalPosition = transform.position;

        rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float topOfCollider = transform.position.y + transform.localScale.y / 2;

        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > topOfCollider)
        {
            StartCoroutine(DelayedDrop());
        }
    }

    private IEnumerator DelayedDrop()
    {
        yield return new WaitForSeconds(dropTimer);
        
        Drop();

        StartCoroutine(DelayedRespawn());
    }

    private void Drop()
    {
        platformCollider.enabled = false;
        rb.isKinematic = false;
    }

    private IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(respawnTimer);

        Respawn();
    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;

        platformCollider.enabled = true;
        rb.isKinematic = true;

        transform.position = originalPosition;
    }

}
