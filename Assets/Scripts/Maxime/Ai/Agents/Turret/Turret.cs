using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour
{
    private BT_Turret m_bt_turret;

    public GameObject bulletPrefab;


    private void Start()
    {
        m_bt_turret = GetComponent<BT_Turret>();
        m_bt_turret.gameObject = gameObject;
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
}
