using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public float health;
    public float maxHealth = 100.0f;
    public Slider healthSlider;
    public GameObject bullet;
    public Transform shootSpawnPoint;

    public virtual void Start()
    {
        health = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
    }

    public void Update()
    {
        healthSlider.value = health / maxHealth;

        if (health <= 0.0f)
        {
            Component[] components = GetComponents<Component>();
            foreach (Component component in components)
            {
                if (!(component is Transform))
                {
                    Destroy(component);
                }
            }
            Destroy(gameObject);
        }
    }
    
    public static event Action OnBroadcastMessage;
    private void SendMessageToPlayerUI()
    {
        OnBroadcastMessage?.Invoke();
    }

    private void OnDestroy()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            SendMessageToPlayerUI();
        }
    }

    public void SpawnBullet()
    {
        GameObject bulletInstance = Instantiate(bullet, shootSpawnPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        bulletScript.damageType = Bullet.DamageType.Friendly;
        Vector3 shootDirection = transform.forward;
        shootDirection.y = 0;
        bulletScript.Initialize(shootDirection);
    }
}
