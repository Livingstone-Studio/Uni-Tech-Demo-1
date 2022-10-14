using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : Pickup
{
    [Header("Components")]

    [SerializeField] private ParticleSystem jetpackEffect;

    [SerializeField] private Slider fuelGaugeUI;

    [Header("Flying Settings")]
    
    [SerializeField] private float jetpackAcceleration = 5f;

    [Header("Fuel Settings")]

    [SerializeField] private float fuelConsumptionRate = 1.5f;
    [SerializeField] private float fuelReplenishRate = 1.5f;
    
    [SerializeField] private float fuelMax = 5f;

    private float fuel;

    private bool fuelNotZero = false;

    protected override void Start()
    {
        fuel = fuelMax;

        if (fuelGaugeUI)
        {
            fuelGaugeUI.minValue = 0;
            fuelGaugeUI.maxValue = fuelMax;

            if (pickupState == PickupState.PICKUP)
            {
                fuelGaugeUI.gameObject.SetActive(false);
            }
        }

        base.Start();
    }

    protected override void Action()
    {
        if (pM == null) return;

        if (pM.moveable == null) return;

        if (fuelGaugeUI)
        {
            if (!fuelGaugeUI.gameObject.activeInHierarchy)
            {
                fuelGaugeUI.gameObject.SetActive(true);
            }
        }

        if (Input.GetButton("Jump") && pM.moveable.moveState != MoveState.LADDER && (pM.moveable.isFalling || pM.moveable.jumpLimit) && !pM.moveable.jumping && fuelNotZero)
        {
            if (jetpackEffect != null)
            {
                if (jetpackEffect.isStopped) jetpackEffect.Play();
            }

            if (UseFuel())
            {
                Fly(pM.moveable);
            }
        }
        else
        {
            if (jetpackEffect != null)
            {
                if (jetpackEffect.isPlaying) jetpackEffect.Stop();
            }

            FuelReplenish();
        }

        if (fuelGaugeUI)
        {
            fuelGaugeUI.value = fuel;
        }
    }

    private void Fly(Moveable moveable)
    {
        if (moveable == null) return;

        moveable.rb.AddForce(Vector2.up * jetpackAcceleration * Time.deltaTime * 1000, ForceMode2D.Force);
    }

    private bool UseFuel()
    {
        if (CameraController.Instance.moving) return false;

        fuel -= Time.deltaTime * fuelConsumptionRate;

        if (fuel <= 0)
        {
            fuel = 0;

            fuelNotZero = false;

            return false;
        }
        else
        {
            return true;
        }
    }

    private bool FuelReplenish()
    {
        if (CameraController.Instance.moving) return false;

        fuel += Time.deltaTime * fuelReplenishRate;

        if (fuel >= fuelMax / 2)
        {
            fuelNotZero = true;
        }

        if (fuel >= fuelMax)
        {
            fuel = fuelMax;

            return true;
        }
        else
        {
            return false;
        }
    }
}
