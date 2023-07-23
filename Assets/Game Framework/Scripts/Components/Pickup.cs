using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Base class for pickups. Pickups Should be an empty game object with a collider and a pickup component
/// with the pickup visuals as a child of the pickup game object.
/// </summary>
public class Pickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;
    public bool hideOnPickup = false;
    public float respawnTime = 10f;
    public GameObject pickupVisuals;

    [Header("Audio")]
    public AudioClip pickupSound;

    public UnityEvent OnPickedUp;

    private bool isPicked = false;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;

    }

    public virtual void OnPickup(Pawn player)
    {
        // Do something when picked up
        isPicked = true;
        OnPickedUp.Invoke();
        StartCoroutine(RespawnPickup());
        // Play pickup sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }

        if (hideOnPickup)
        {
            pickupVisuals.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pawn pawn = other.GetComponent<Pawn>();
        if (pawn != null && !isPicked)
        {
            OnPickup(pawn);
        }
    }

    private IEnumerator RespawnPickup()
    {
        yield return new WaitForSeconds(respawnTime);
        isPicked = false;
        pickupVisuals.SetActive(true);
    }

}
