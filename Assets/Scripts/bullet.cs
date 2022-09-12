using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    public int speed;
    public int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = gameManager.instance.player.transform.position - rb.position;
        rb.velocity = direction * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
