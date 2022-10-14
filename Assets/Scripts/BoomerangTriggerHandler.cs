using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangTriggerHandler : MonoBehaviour
{
    [SerializeField] private Boomerang boomerang;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boomerang.TriggerEnter(collision);
    }
}
