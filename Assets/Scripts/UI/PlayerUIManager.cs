using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public TextMeshProUGUI killCountText;
    private int m_killCount = 0;
    

    private void OnEnable()
    {
        Agent.OnBroadcastMessage += EnemyDeadOnReceiveMessage;
    }
    
    private void OnDisable()
    {
        Agent.OnBroadcastMessage -= EnemyDeadOnReceiveMessage;
    }
    
    private void EnemyDeadOnReceiveMessage()
    {
        m_killCount++;
        killCountText.text = " " + m_killCount;
    }
}
