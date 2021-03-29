using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent = null;

    Camera mainCamera;
    public CharacterController PC;
    public float speed;
    
    void Update()
    {
        CmdMove();
    }
    [Command]
    private void CmdMove()
    {
        if (!isLocalPlayer) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        PC.Move(dir * speed * Time.deltaTime);
        
    }
/*
    #region Server
    [Command]
    private void CmdMove(Vector3 position)
    {
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }
        agent.SetDestination(hit.position);
    }
    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetMouseButton(1)) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        CmdMove(hit.point);
    }
    #endregion
*/
}