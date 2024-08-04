using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
            if (hit.collider == null) return;
            if (hit.collider.gameObject.GetComponent<Interactable>() == null
                 && hit.collider.transform.parent == null) return;

            var interactionTarget = hit.collider.gameObject.GetComponent<Interactable>();
            if(interactionTarget == null) interactionTarget = hit.collider.transform.parent.GetComponent<Interactable>();
            switch (interactionTarget.interactableType) 
            {
                case InteractableType.PICKUP:
                    var item = interactionTarget.GetComponent<Pickup>().Item;
                    if (inventory.ContainsKey(item)) inventory[item]++;
                    else inventory.Add(item, 1);
                    UpdateItemAmounts(item, inventory[item]);
                break;

                case InteractableType.TALK:
                    var npcTarget = interactionTarget.GetComponent<NPC>();
                    DisplayText(interactionTarget.gameObject, npcTarget.DialogueText);
                break;

                case InteractableType.DELIVER:
                    var deliveryTarget = interactionTarget.GetComponent<DeliveryTarget>();
                    if (inventory.ContainsKey(deliveryTarget.DeliveryItem))
                    {
                        deliveryTarget.DeliveryCounter += inventory[deliveryTarget.DeliveryItem];
                        inventory[deliveryTarget.DeliveryItem] = 0;
                    }
                    UpdateItemAmounts(deliveryTarget.DeliveryItem, inventory[deliveryTarget.DeliveryItem]);
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
    [SerializeField] float rotationSpeed;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] GameObject playerModel;
    Vector3 moveDirection = Vector3.zero;

    Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();

    // Component References

    private void Awake()
    {
        // Move to scene Start
        transform.position = GameObject.Find("Scene Start").transform.position;

        // Initialize Input Component
        input = new PlayerInputActions();

        // Initialize Inventory
        inventory = new Dictionary<ItemType, int> {
            {ItemType.LEAF, 0},
            {ItemType.ROCK, 0},
            {ItemType.FLAG, 0}
        };
        foreach (var item in inventory) UpdateItemAmounts(item.Key, inventory[item.Key]);
    }

    private void Update()
    {
        // Apply movement vector
        var v2Input = move.ReadValue<Vector2>();
        var v2Move = v2Input;
        if (v2Move != Vector2.zero) {
            // Normalize vectors
            var forward = camera.transform.forward;
            var right = camera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;

            // Make input relative to camera
            var relativeX = v2Move.x * right;
            var relativeY = v2Move.y * forward;
            moveDirection = relativeX + relativeY;

            // Apply movement to controller
            this.GetComponent<CharacterController>().Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        // Lerp player rotation
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed * Time.deltaTime);

    }

    public void DisplayText(GameObject speaker, string text)
    {
        playerUI.DisplayDialogue(text, speaker);
    }

    public void UpdateItemAmounts(ItemType item, int amount)
    {
        playerUI.SetResource(item, amount);
    }
}
