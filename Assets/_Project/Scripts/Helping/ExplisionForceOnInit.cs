using UnityEngine;

public class ExplisionForceOnInit : MonoBehaviour
{
    public float radius = 10;
    public float power = 10;
    public float damage = 10;

    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);

            hit.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

}
