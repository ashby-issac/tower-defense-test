using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 50f; // projectile damage to the enemy
    [SerializeField] private Rigidbody rb;

    private float timer;
    private Vector3 direction;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        CheckLifetime();
    }

    public void MoveProjectile(Vector3 dir)
    {
        rb.velocity = dir * moveSpeed;
    }

    private void CheckLifetime()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            HandleDestruction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && other.gameObject.tag == "Enemy")
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void HandleDestruction()
    {
        // Push to pool 
        gameObject.SetActive(false);
    }
}
