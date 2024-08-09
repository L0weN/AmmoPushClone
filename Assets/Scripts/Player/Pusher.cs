using Mert.EventBus;
using UnityEngine;

/// <summary>
/// This class is responsible for pushing rigidbodies when the player collides with them.
/// </summary>
public class Pusher : MonoBehaviour
{
    public LayerMask pushLayers; // The layers that the pusher can push.
    public bool canPush; // If the pusher can push rigidbodies.
    
    private BoxCollider boxCollider; // The box collider of the pusher.
    private float minBoxColliderSize = 7.0f; // The minimum size of the box collider.

    private float power; // The power of the pusher.
    private float minPowerLevel = 1.1f; // The minimum power level of the pusher.

    EventBinding<PowerUpPurchased> powerUpPurchasedEventBinding;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        InitializePusher();
    }

    private void OnEnable()
    {
        powerUpPurchasedEventBinding = new EventBinding<PowerUpPurchased>(HandlePowerUpPurchased);
        EventBus<PowerUpPurchased>.Register(powerUpPurchasedEventBinding);
    }

    private void OnDisable()
    {
        EventBus<PowerUpPurchased>.Unregister(powerUpPurchasedEventBinding);
    }

    private void HandlePowerUpPurchased(PowerUpPurchased powerUpPurchased)
    {
        if (powerUpPurchased.PowerUpData.powerUpName == "Size")
        {
            SetPusherSize();
        }
        else if (powerUpPurchased.PowerUpData.powerUpName == "Power")
        {
            SetPusherPower();
        }
    }

    private void InitializePusher()
    {
        if (boxCollider != null)
        {
            SetPusherSize();
            SetPusherPower();
        }
    }

    private void SetPusherSize()
    {
        Vector3 size = new Vector3(minBoxColliderSize + GameResources.Instance.GetPowerUpLevel("Size"), boxCollider.size.y, boxCollider.size.z);
        boxCollider.size = size;
    }

    private void SetPusherPower()
    {
        power = minPowerLevel + GameResources.Instance.GetPowerUpLevel("Power");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canPush) PushRigidBodies(hit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPush) PushRigidBodies(collision);
    }

    private void PushRigidBodies(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        if (hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        body.AddForce(pushDir * power, ForceMode.Impulse);
    }

    private void PushRigidBodies(Collision collision)
    {
        Rigidbody body = collision.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        if (collision.GetContact(0).normal.y < -0.3f) return;

        Vector3 pushDir = new Vector3(collision.GetContact(0).normal.x, 0.0f, collision.GetContact(0).normal.z);

        body.AddForce(pushDir * power, ForceMode.Impulse);
    }
}
