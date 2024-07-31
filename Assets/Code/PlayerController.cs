using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (hit.collider == null || hit.collider.gameObject.GetComponent<Interactable>() == null) return;

            var interactionTarget = hit.collider.gameObject.GetComponent<Interactable>();
            switch (interactionTarget.interactableType) 
            {
                case InteractableType.PICKUP:
                    var item = interactionTarget.GetComponent<Pickup>().Item;
                    if (inventory.ContainsKey(item)) inventory[item]++;
                    else inventory.Add(item, 1);
                    Debug.Log(item + inventory[item]);
                break;

                case InteractableType.TALK:
                break;

                case InteractableType.DELIVER:
                    var deliveryTarget = interactionTarget.GetComponent<DeliveryTarget>();
                    if (inventory.ContainsKey(deliveryTarget.DeliveryItem))
                    {
                        deliveryTarget.DeliveryCounter += inventory[deliveryTarget.DeliveryItem];
                        inventory[deliveryTarget.DeliveryItem] = 0;
                    }
                break;
            }
            interactionTarget.DoInteraction(this);
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

    Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();

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
