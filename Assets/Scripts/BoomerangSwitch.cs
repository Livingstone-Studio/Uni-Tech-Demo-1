using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BoomerangSwitch : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sR;

    [SerializeField] private UnityEvent OnBoomerangHit;

    private void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boomerang"))
        {
            if (collision.TryGetComponent<Boomerang>(out Boomerang boomerang))
            {
                if (!boomerang.Holstered())
                {
                    OnBoomerangHit.Invoke();
                }
            }
        }
    }

    public void ChangeColour()
    {
        if (sR == null) return;

        sR.color = Color.green;
    }
}
