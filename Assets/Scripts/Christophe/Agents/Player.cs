using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Agent
{
    public enum FormatType
    {
        Circle,
        Squad
    }
    public FormatType format;
    
    public float moveSpeed = 0.5f;
    public RectTransform crosshairUI;
    public float formatDistance = 1.5f;
    public List<GameObject> NPCs = new List<GameObject>();
    [FormerlySerializedAs("Guardians")] public List<GameObject> guardians = new List<GameObject>();
    
    public void AddNewNPC(GameObject npc)
    {
        NPCs.Add(npc);
    }
    
    public override void Start()
    {
        base.Start();
        format = FormatType.Squad;
        inputActions = new PlayerControllerInputMapping();
        inputActions.PlayerKeyBoard.Enable();
        Cursor.visible = false;
    }

#region ActionInputs
    
    private PlayerControllerInputMapping inputActions;

    private void GetExitInput()
    {
        if (inputActions.PlayerKeyBoard.Exit.WasPressedThisFrame())
        {
            // Use pre processor macro because Build doesn't recognize EditorApplication
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    private void GetMoveInput()
    {
        Vector2 moveVector2 = inputActions.PlayerKeyBoard.Movement.ReadValue<Vector2>();
        if (moveVector2  != Vector2.zero) 
        {
            Move(moveVector2.x, moveVector2.y);
        }
    }

    private void Move(float horizontal, float vertical)
    {
        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * moveSpeed, Space.World);
    }

    public static bool isGaming = true;
    private void GetAttackInput()
    {
        if (inputActions.PlayerKeyBoard.Attack.WasPressedThisFrame())
        {
            if (isGaming)
            {
                SendMessageToNPCs();
                SpawnBullet();
            }
        }
    }

    private void SetNPCsAutoAttack()
    {
        if (inputActions.PlayerKeyBoard.AutoAttack.WasPressedThisFrame())
        {
            BT_Attacker.shootPos = shootPos; // Give last shot pos to Attackers
            BT_Attacker.isAutoAttacking = true;
        }
    }
    
    private void GetChangeFormatInput()
    {
        if (inputActions.PlayerKeyBoard.ChangeFormat.WasPressedThisFrame())
        {
            format = (format == FormatType.Squad)? 0 : format = FormatType.Squad;
        }
    }

    public LayerMask ignoreEnemyLayer;
    public Vector3 shootPos = Vector3.zero;
    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = inputActions.PlayerKeyBoard.MousMove.ReadValue<Vector2>();
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit; 
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreEnemyLayer))
        {
            Vector3 targetPosition = hit.point;
            
            shootPos = targetPosition;

            shootPos.y = transform.position.y;

            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; 

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }

        if (crosshairUI == null)
        {
            crosshairUI = GameObject.Find("CrossHair").GetComponent<RectTransform>();
        }
        
        crosshairUI.position = mousePosition;
    }

    void CallGuardians()
    {
        if (inputActions.PlayerKeyBoard.CallGuardians.WasPressedThisFrame())
        {
            health -= 30;
            BT_Guardian.isDefending = true;
        }
    }

    void CallBackAttackers()
    {
        if (inputActions.PlayerKeyBoard.CallBackAttackers.WasPerformedThisFrame())
        {
            BT_Attacker.isAutoAttacking = false;
        }
    }

    void Dash()
    {
        if (inputActions.PlayerKeyBoard.Dash.WasPerformedThisFrame())
        {
            moveSpeed *= 2;
        }

        if (inputActions.PlayerKeyBoard.Dash.WasReleasedThisFrame())
        {
            moveSpeed *= 0.5f;
        }
    }

    void InputActions()
    {
        GetExitInput();
        GetMoveInput();
        GetAttackInput();
        SetNPCsAutoAttack();
        GetChangeFormatInput();

        RotateTowardsMouse();
        CallGuardians();
        CallBackAttackers();
        Dash();
    }
#endregion ActionInputs

    public void Update()
    {
        base.Update();
        InputActions();
        Dead();
    }

    void Dead()
    {
        if (health <= 0)
        {
            SendMessageToNPCs();
            foreach (var npc in NPCs)
            {
                Destroy(npc);
            }
            Destroy(gameObject);
        }
    }

#region MessageSend
    
    public static event Action OnBroadcastMessage;
    private void SendMessageToNPCs()
    {
        OnBroadcastMessage?.Invoke();
    }
    
#endregion MessageSend
}
