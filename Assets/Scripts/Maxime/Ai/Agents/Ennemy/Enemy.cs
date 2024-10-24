using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    private BT_Enemy m_bt_enemy;

    public GameObject bulletPrefab;
    
    
    private void Start()
    {
        m_bt_enemy = GetComponent<BT_Enemy>();
        m_bt_enemy.gameObject = gameObject;
    }
    

    public void Shoot()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        bulletScript.damageType = Bullet.DamageType.FromEnemy;

        Vector3 shootDirection = transform.forward;
        shootDirection.y = 0;
        bulletScript.Initialize(shootDirection);
    }

    private void GeneratePoint()
    {
        Vector2 randomPos2D = Random.insideUnitCircle * 10.0f;
        Vector3 randomPos3D = transform.position + new Vector3(randomPos2D.x, 0.0f, randomPos2D.y);
    }
}
