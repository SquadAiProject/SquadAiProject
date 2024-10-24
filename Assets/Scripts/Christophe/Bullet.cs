using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum DamageType
    {
        FromEnemy,
        Friendly,
    }

    public DamageType damageType;

    public float damage = 5.0f;
    public float speed = 30.0f; 
    
    private Vector3 direction;
    
    public void Initialize(Vector3 shootDirection)
    {
        float randomOffsetX = Random.Range(-0.05f, 0.05f); 
        float randomOffsetY = Random.Range(-0.05f, 0.05f); 
        Vector3 randomOffset = new Vector3(randomOffsetX, randomOffsetY, 0);

        direction = (shootDirection + randomOffset).normalized;
    }
    
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    } 
    
    public GameObject hitEffectParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
        
        switch (damageType)
        {
            case DamageType.Friendly:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    CapsuleCollider enemyCollider = other as CapsuleCollider;
                    if (enemyCollider != null)
                    {
                        DoDamage(other);
                        Destroy(GameObject.Instantiate(shootSound, transform.position, Quaternion.identity), 0.8f);      
                        Destroy(gameObject);
                    }
                }
                break;
            
            case DamageType.FromEnemy:
                if (other.gameObject.CompareTag("Player"))
                {
                    DoDamage(other);
                    Destroy(GameObject.Instantiate(shootSound, transform.position, Quaternion.identity), 0.8f);      
                    Destroy(gameObject);
                }
                else if (other.gameObject.CompareTag("NPC"))
                {
                    DoDamage(other);
                    Destroy(GameObject.Instantiate(shootSound, transform.position, Quaternion.identity), 0.8f);      
                    Destroy(gameObject);
                }
                else if (other.gameObject.CompareTag("Shield"))
                {
                    Destroy(GameObject.Instantiate(shootSound, transform.position, Quaternion.identity), 0.8f);      
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void DoDamage(Collider other)
    {
        GameObject hitEffectParticle = Instantiate(hitEffectParticlePrefab, transform.position, Quaternion.identity);
        Destroy(hitEffectParticle, 0.5f);
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().health -= damage;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Agent>().health -= damage;
        }
        else if (other.gameObject.CompareTag("NPC"))
        {
            other.GetComponent<Agent>().health -= damage;
        }
    }

    public GameObject shootSound;
    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }
}
