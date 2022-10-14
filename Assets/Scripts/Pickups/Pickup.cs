using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupState { PICKUP, ACTIVE }

[RequireComponent(typeof(Collider2D))]
public abstract class Pickup : MonoBehaviour
{

    public PickupState pickupState = PickupState.PICKUP;

    [SerializeField] protected int pickupIndex = 0;

    protected Collider2D pickupCollider;

    protected Transform posOnPlayer;

    protected PickupManager pM;

    protected virtual void Start()
    {
        pickupCollider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (pickupState == PickupState.ACTIVE)
        {
            Action();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickupState == PickupState.PICKUP && collision.CompareTag("Player"))
        {
            pM = collision.gameObject.GetComponent<PickupManager>();

            if (pM == null) return;

            posOnPlayer = pM.pickupTransforms[pickupIndex];

            pickupCollider.enabled = false;

            transform.parent = posOnPlayer;

            transform.position = posOnPlayer.position;

            pickupState = PickupState.ACTIVE;
        }
    }

    protected virtual void Action()
    {
        // Whatever pickup does while active.
    }
}
