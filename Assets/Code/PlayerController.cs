using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Input

    PlayerInputActions input;

    InputAction interact;
    InputAction mouse;
    InputAction move;

    private void OnEnable()
    {
        // Set up inputs and enable them
        interact = input.Player.Interact;
        mouse = input.Player.Mouse;
        move = input.Player.Move;

        // Enable controls
        interact.Enable();
        mouse.Enable();
        move.Enable();

        // Input action for interact
        input.Player.Interact.performed += OnInteract;
        input.Player.Interact.Enable();

        // Input action for move
        //input.Player.Move.performed += OnMove;
        input.Player.Move.Enable();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        // Get the object under the cursor
        var v2 = (mouse.ReadValue<Vector2>());
        var ray = camera.ScreenPointToRay(mouse.ReadValue<Vector2>());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != null) Debug.Log(hit.collider.gameObject);

        }
    }

    /* Old code
    void OnMove(InputAction.CallbackContext context)
    {
        var v2 = context.ReadValue<Vector2>() * 4f * Time.deltaTime;
        this.transform.position += new Vector3(v2.x, 0f, v2.y);
    }
    */
    #endregion

    // Serialized Fields
    [SerializeField] Camera camera;
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        // Move to scene Start
        transform.position = GameObject.Find("Scene Start").transform.position;

        // Initialize Input Component
        input = new PlayerInputActions();
    }

    private void Update()
    {
        // Apply movement vector
        var v2 = move.ReadValue<Vector2>() * moveSpeed * Time.deltaTime;
        if (v2 != Vector2.zero) {
            // Normalize vectors
            var forward = camera.transform.forward;
            var right = camera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;

            // Make input relative to camera
            var relativeX = v2.x * right;
            var relativeY = v2.y * forward;

            // Apply movement to controller
            this.GetComponent<CharacterController>().Move(relativeX + relativeY);
        }
    }
}
