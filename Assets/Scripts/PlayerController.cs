using System;
using UnityEngine;
using HorseMoon.Inventory;
using HorseMoon.Inventory.ItemTypes;
using HorseMoon.Tools;
using HorseMoon.GameMenuUI;

namespace HorseMoon
{

    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerController : CharacterControl
    {
        // TODO: Remove when inventory is done.
        public Tool[] tools;
        public Tool selectedTool;
        public SeedTool seedTool;
        public ItemTool itemTool;
        public Transform toolDirectionMarker;
        public Transform toolHolder;

        private InteractionObject targetObject;

        private Player player;
        private new SpriteRenderer renderer;
        private GameObject toolObject;
        public Bag bag;

        public Vector2 ToolMarkerOffset { get; private set; } = Vector2.one; // with drag to smoothing out the controls
        public Vector2Int ToolDirection { get; private set; }


        public Vector2Int FacingTile => TilePosition + ToolDirection;

        public bool freezeMovement;
        bool StopMovement => freezeMovement || stopUntil > Time.timeSinceLevelLoad;
        float stopUntil;

        protected void Start()
        {
            player = GetComponent<Player>();
            renderer = GetComponent<SpriteRenderer>();
            SelectTool(tools[0]);
        }

        protected void OnEnable()
        {
            bag = GetComponent<Bag>();
            bag.ItemChanged += OnBagItemChanged;
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            bag.ItemChanged -= OnBagItemChanged;
        }

        private void Update()
        {
            if (!StopMovement && Input.GetButtonDown("Pause"))
                GameMenu.Instance.Open();

            HandleTargeting();
        }

        void HandleToolDirection(float horizontal, float vertical)
        {
            var currentInput = new Vector2(horizontal, vertical);
            var cin = currentInput.normalized;
            if (currentInput.sqrMagnitude > 0f)
            {
                var currentInputExtreme = new Vector2Int(Mathf.RoundToInt(cin.x), Mathf.RoundToInt(cin.y));
                if (currentInputExtreme.x != 0 && ToolDirection.x != currentInputExtreme.x)
                {
                    ToolDirection = currentInputExtreme;
                    ToolMarkerOffset = ToolDirection;
                }
                else if (currentInputExtreme.y != 0 && ToolDirection.y != currentInputExtreme.y)
                {
                    ToolDirection = currentInputExtreme;
                    ToolMarkerOffset = ToolDirection;
                }
                else
                {
                    ToolMarkerOffset = ToolMarkerOffset * .8f + cin * .2f;
                    ToolDirection = new Vector2Int(Mathf.RoundToInt(ToolMarkerOffset.x), Mathf.RoundToInt(ToolMarkerOffset.y));
                }
            }

            if (ToolDirection.x != 0f)
                transform.SetForward(ToolDirection.x);

            toolHolder.position = transform.position + (Vector3)ToolMarkerOffset.normalized;

        }


        private void HandleTargeting()
        {
            if (StopMovement)
                return;

            Collider2D hit = Physics2D.Raycast(transform.position, ToolDirection, 1.5f, LayerMask.GetMask("Default"))
        .collider;
            InteractionObject hitObject = null;
            if (hit != null)
            {
                hitObject = hit.GetComponent<InteractionObject>();
            }
            if (targetObject != null && targetObject != hitObject)
            {
                targetObject.MarkTargeted(false);
                targetObject = null;
            }

            bool canUseOnObject = hitObject != null && selectedTool != null && selectedTool.CanUse(player, hitObject);
            bool canUseOnTile = !canUseOnObject && selectedTool != null && selectedTool.CanUse(player, FacingTile);
            bool canUseObject = hitObject != null && hitObject.CanUse(player);
            bool shouldTargetObject = canUseOnObject || canUseObject;
            if (shouldTargetObject)
            {
                hitObject.MarkTargeted(true);
                targetObject = hitObject;
            }
            toolDirectionMarker.gameObject.SetActive(!shouldTargetObject && canUseOnTile);
            toolDirectionMarker.position = FacingTile + new Vector2(0.5f, 0.5f);


            if (Input.GetButtonDown("Use"))
            {
                if (shouldTargetObject)
                {
                    // Only use the object if we can't use our tool and the object says it can be used.
                    if (canUseOnObject)
                    {

                        selectedTool.UseTool(player, hitObject, toolObject);
                        selectedTool.PlayAudio();
                        stopUntil = Time.timeSinceLevelLoad + selectedTool.cooldown;
                    }
                    else
                        hitObject.UseObject(player);
                }
                else if (canUseOnTile)
                {
                    selectedTool.PlayAudio();
                    selectedTool.UseTool(player, FacingTile, toolObject);
                    stopUntil = Time.timeSinceLevelLoad + selectedTool.cooldown;
                }
            }
            else if (Input.GetButtonDown("Use Item"))
            {
                if (selectedTool == itemTool && itemTool.itemInfo is FoodInfo foodInfo)
                {
                    player.Stamina += foodInfo.energy;
                    bag.Remove(foodInfo, 1);
                    selectedTool.PlayAudio();
                    stopUntil = Time.timeSinceLevelLoad + selectedTool.cooldown;
                }
            }
        }


        public void SelectTool(Tool tool)
        {
            if (selectedTool != null)
            {
                selectedTool.OnUnequipped();
            }
            if (toolObject != null)
            {
                Destroy(toolObject);
            }
            selectedTool = tool;
            if (selectedTool != null && selectedTool.toolPrefab != null)
            {
                toolObject = Instantiate(tool.toolPrefab, toolHolder);
                selectedTool.OnEquipped();
            }
        }

        public void SetToolForItem(Item item)
        {
            ItemInfo itemInfo = item.info;

            if (itemInfo is ToolInfo toolInfo)
            {
                SelectTool(tools[(int)toolInfo.type]);
            }
            else if (itemInfo is SeedInfo seedInfo)
            {
                seedTool.cropData = seedInfo.plantType;
                seedTool.seedItem = item;
                SelectTool(seedTool);
            }
            else
            {
                itemTool.itemInfo = itemInfo;
                SelectTool(itemTool);
                toolObject.GetComponent<SpriteRenderer>().sprite = itemInfo.sprite;
            }
        }

        private void OnBagItemChanged(Item item)
        {
            // Unequip the SeedTool when it plants the last seed it has. -->
            if (selectedTool == seedTool)
            {
                if (item == seedTool.seedItem && item.Quantity <= 0)
                    SelectTool(tools[0]);
            }
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float horizontal2 = Input.GetAxis("Horizontal2");
            float vertical2 = Input.GetAxis("Vertical2");
            Vector2 moveInput = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
            Vector2 rightStockInput = new Vector2(horizontal2, vertical2);
            if (Input.GetButton("Stand") || StopMovement)
                Move(Vector2.zero);
            else
                Move(moveInput);

            if (StopMovement)
            { }
            else if (rightStockInput == Vector2.zero)
                HandleToolDirection(horizontal, vertical);
            else
                HandleToolDirection(horizontal2, vertical2);
        }
    }
}