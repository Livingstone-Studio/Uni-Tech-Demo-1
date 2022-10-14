using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boomerang : Pickup
{
    [Header("Components")]

    private Rigidbody2D rb;

    [SerializeField] private Collider2D replacedPickupCollider;

    [Header("Boomerang Settings")]

    [SerializeField] private float throwForce = 5f;

    private bool holstered = false;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.isKinematic = true;

        pickupCollider = replacedPickupCollider;
    }

    protected override void Action()
    {
        if (CameraController.Instance.moving && rb.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (!CameraController.Instance.moving && rb.constraints != RigidbodyConstraints2D.FreezeRotation)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (Input.GetButtonDown("Fire1") && holstered)
        {
            transform.parent = null;

            rb.isKinematic = false;

            pickupCollider.enabled = false;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z = 0;

            Vector2 dir = (Vector2)mousePosition - (Vector2)pM.transform.position;

            rb.velocity = Vector2.zero;

            holstered = false;

            rb.AddForce(throwForce * dir.normalized, ForceMode2D.Impulse);

            StartCoroutine(DelayedCollider());
        }
        
        if (!holstered)
        {
            Vector2 dir = (Vector2)pM.transform.position - (Vector2)transform.position;

            rb.AddForce(Time.deltaTime * 1000 * dir.normalized, ForceMode2D.Force);
        }
    }

    private IEnumerator DelayedCollider()
    {
        yield return new WaitForSeconds(0.4f);

        pickupCollider.enabled = true;
    }

    public void TriggerEnter(Collider2D collision)
    {
        if (pickupState == PickupState.ACTIVE && collision.CompareTag("Player"))
        {
            posOnPlayer = pM.pickupTransforms[pickupIndex];

            pickupCollider.enabled = false;

            transform.parent = posOnPlayer;

            transform.position = posOnPlayer.position;

            rb.velocity = Vector2.zero;

            rb.isKinematic = true;

            holstered = true;
        }

        if (pickupState == PickupState.PICKUP && collision.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            holstered = true;
        }

        base.OnTriggerEnter2D(collision);
    }

    public bool Holstered()
    {
        return holstered;
    }
}