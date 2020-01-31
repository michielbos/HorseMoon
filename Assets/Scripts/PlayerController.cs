using System;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    // TODO: Remove when inventory is done.
    public Tool[] tools;
    private int toolIndex = 0;
    public Tool selectedTool;
    public Vector2Int toolDirection;
    public Transform toolDirectionMarker;

    private InteractionObject targetObject;

    private CharacterControl characterController;
    private Player player;
    private new SpriteRenderer renderer;

    public Vector2Int TilePosition => characterController.TilePosition;

    public Vector2Int FacingTile => TilePosition + toolDirection;

    private void Start() {
        characterController = GetComponent<CharacterControl>();
        player = GetComponent<Player>();
        renderer = GetComponent<SpriteRenderer>();
        SelectTool(tools[0]);
    }

    private void Update() {
        // TODO: Replace with inventory system.
        /*if (Input.GetButtonDown("Previous Item")) {
            if (--toolIndex < 0)
                toolIndex = tools.Length - 1;
            SelectTool(tools[toolIndex]);
        }
        if (Input.GetButtonDown("Next Item")) {
            if (++toolIndex >= tools.Length)
                toolIndex = 0;
            SelectTool(tools[toolIndex]);
        }*/
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
                selectedTool.UseTool(player, hitObject);
            } else {
                selectedTool.UseTool(player, FacingTile);
            }
        }
    }

    public void SelectTool(Tool tool) {
        selectedTool = tool;
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