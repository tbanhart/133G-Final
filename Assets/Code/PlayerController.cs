using System;
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

            var hitTarget = hit.collider.gameObject;
            var interactionTarget = hit.collider.gameObject.GetComponent<Interactable>();
            
            var counter = 0;
            while (counter < 4 && interactionTarget == null)
            {
                Debug.Log("Object " + hitTarget.name + " didn't have interactable. Looking into next.");
                hitTarget = hitTarget.transform.parent.gameObject;
                try
                {
                    interactionTarget = hitTarget.GetComponent<Interactable>();
                }
                catch 
                {
                }
                counter++;
            }
            // This may cause issues if there are things after interactions
            if (counter == 4) return;
            switch (interactionTarget.interactableType) 
            {
                case InteractableType.PICKUP:
                    var item = interactionTarget.GetComponent<Pickup>().Item;
                    if (Inventory.ContainsKey(item)) Inventory[item]++;
                    else Inventory.Add(item, 1);
                    UpdateItemAmounts(item, Inventory[item]);
                break;

                case InteractableType.TALK:
                    var npcTarget = interactionTarget.GetComponent<NPC>();
                    DisplayText(interactionTarget.gameObject, npcTarget.DialogueText);
                break;

                case InteractableType.DELIVER:
                    var deliveryTarget = interactionTarget.GetComponent<DeliveryTarget>();
                    if (Inventory.ContainsKey(deliveryTarget.DeliveryItem))
                    {
                        deliveryTarget.DeliveryCounter += Inventory[deliveryTarget.DeliveryItem];
                        Inventory[deliveryTarget.DeliveryItem] = 0;
                        deliveryTarget.DoDelivery();
                    }
                    UpdateItemAmounts(deliveryTarget.DeliveryItem, Inventory[deliveryTarget.DeliveryItem]);
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
    Animator animator;
    CharacterController controller;
    public string CurrentScene;
    public bool FreezeObject = false;

    public Dictionary<ItemType, int> Inventory = new Dictionary<ItemType, int>();

    // Component References

    private void Awake()
    {
        //DontDestroyOnLoad(this);

        // Move to scene Start
        //transform.position = GameObject.Find("Scene Start").transform.position;

        // Initialize Components
        input = new PlayerInputActions();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Initialize Inventory
        /*
        Inventory = new Dictionary<ItemType, int> {
            {ItemType.LEAF, 0},
            {ItemType.ROCK, 0},
            {ItemType.FLAG, 0}
        };
        */
        //foreach (var item in inventory) UpdateItemAmounts(item.Key, inventory[item.Key]);
    }

    private void Update()
    {
        if(FreezeObject == true)
        {
            FreezeObject = false;
            return;
        }

        // Get move input
        var v2Input = move.ReadValue<Vector2>();
        var v2Move = v2Input;

        // Animation Update
        float xInput = v2Input.x;
        float zInput = v2Input.y;
        animator.SetFloat("InputZ", zInput, 0.3f, Time.deltaTime * 2f);
        animator.SetFloat("InputX", xInput, 0.3f, Time.deltaTime * 2f);
        float speed = new Vector2(xInput, zInput).sqrMagnitude;
        Debug.Log(speed);
        animator.SetFloat("InputMagnitude", speed, 0.3f, Time.deltaTime * 2f);
        Vector3 moveFinal = new Vector3();

        // Apply Movement
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

            moveFinal += moveDirection * moveSpeed;
        }
        // Apply gravity
        if(!controller.isGrounded) { moveFinal.y += Physics.gravity.y; }

        // Apply movement to controller
        controller.Move(moveFinal * Time.deltaTime);

        // Lerp player rotation
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
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