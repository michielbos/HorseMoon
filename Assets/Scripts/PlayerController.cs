using System;
using UnityEngine;
using HorseMoon.Inventory;
using HorseMoon.Inventory.ItemTypes;
using HorseMoon.Tools;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    // TODO: Remove when inventory is done.
    public Tool[] tools;
    private int toolIndex = 0;
    public Tool selectedTool;
    public SeedTool seedTool;
    public Vector2Int toolDirection;
    public Transform toolDirectionMarker;
    public Transform toolHolder;

    private InteractionObject targetObject;

    private CharacterControl characterController;
    private Player player;
    private new SpriteRenderer renderer;
    private GameObject toolObject;

    public Vector2Int TilePosition => characterController.TilePosition;

    public Vector2Int FacingTile => TilePosition + toolDirection;

    private void Start() {
        characterController = GetComponent<CharacterControl>();
        player = GetComponent<Player>();
        renderer = GetComponent<SpriteRenderer>();
        SelectTool(tools[0]);
    }

    private void Update() {
        HandleTargeting();
    }

    private void HandleTargeting() {
            if (toolDirection.x != 0f)
                transform.SetForward(toolDirection.x);// renderer.flipX = toolDirection.x > 0;
        
        Collider2D hit = Physics2D.Raycast(transform.position, toolDirection, 1.5f, LayerMask.GetMask("Default")).collider;
        InteractionObject hitObject = null;
        if (hit != null) {
            hitObject = hit.GetComponent<InteractionObject>();
        }
        if (targetObject != null && targetObject != hitObject) {
            targetObject.MarkTargeted(false);
            targetObject = null;
        }

        bool canUseObject = hitObject != null && selectedTool != null && selectedTool.CanUse(player, hitObject);
        bool canUseTile = !canUseObject && selectedTool != null && selectedTool.CanUse(player, FacingTile);
        if (canUseObject) {
            hitObject.MarkTargeted(true);
            targetObject = hitObject;
        }
        toolDirectionMarker.gameObject.SetActive(!canUseObject && canUseTile);
        toolDirectionMarker.position = FacingTile + new Vector2(0.5f, 0.5f);
        
        if (Input.GetButtonDown("Use") && selectedTool != null) {
            if (hitObject != null) {
                selectedTool.UseTool(player, hitObject, toolObject);
            } else {
                selectedTool.UseTool(player, FacingTile, toolObject);
            }
        }
    }

    public void SelectTool(Tool tool) {
        selectedTool = tool;
        if (toolObject != null) {
            Destroy(toolObject);
        }
        if (selectedTool != null && selectedTool.toolPrefab != null) {
            toolObject = Instantiate(tool.toolPrefab, toolHolder);
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
            SelectTool(seedTool);
        }
        else
            SelectTool(tools[0]);
    }

    private void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(horizontal, vertical);
        moveInput.Normalize();
        characterController.Move(moveInput);

        Vector2Int direction = new Vector2Int(
            horizontal > 0.1f ? 1 : (horizontal < -0.1f ? -1 : 0),
            vertical > 0.1f ? 1 : (vertical < -0.1f ? -1 : 0)
        );
        if (direction != Vector2Int.zero)
            toolDirection = direction;
    }
}

}