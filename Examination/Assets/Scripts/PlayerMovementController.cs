using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private CharacterController controller = null;

    private Vector2 previousInput;

    private Controls controls;
    private Controls Controls
    {
        get
        {
            //RETRIES TO GET CONTROLS IF FAILED
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();
    }
    //MAKES SURE THAT THE ONLY ONE WHO CAN RUN THIS IS THE LOCAL CLIENT
    #region ClientCallBacks
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    [ClientCallback]
    private void Update() => Move();
    #endregion
    //ACTUALL MOVEMENT CODE
    #region Movement
    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void Move()
    {
        //1. SETS THE VECTORS TO CONTROLER AND REMOVES Y
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        //2. APPLIES THE VECTORS TO PLAYER
        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;
        controller.Move(movement * movementSpeed * Time.deltaTime);
    }
    #endregion Client;
}
