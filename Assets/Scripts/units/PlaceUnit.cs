using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceUnit : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject unitToPlace;
    [SerializeField] private GameObject focusOverlay;

    private Tilemap tilemap;
    private SpriteRenderer[] directionSprites;
    private SpriteRenderer focusOverlaySprite;
    private SpriteRenderer rangeOverlaySprite;

    [SerializeField] private Vector3 spriteOffset = new Vector3(.5f, .5f, 0f);

    private GameObject placedUnit;
    // replace this with a real grid data structure
    private List<Vector3> unitLocations = new List<Vector3>();
    bool rotatingUnit = false;



    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        focusOverlaySprite = focusOverlay.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for mouse down, place unit if clicked
        if (!rotatingUnit) {
            handlePlacementClick();
        }

        // let user rotate unit after placed
        if (rotatingUnit) {
            handleRotationSelect();
        }
    }

    void handlePlacementClick() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilemapPos = tilemap.WorldToCell(clickPos);
            Tile clickedTile = tilemap.GetTile<Tile>(tilemapPos);
            
            // tile was not the correct type to place
            if (clickedTile == null) {
                return;
            } else {
                // create unit sprite and get bottom left location from clicked tile
                Vector3 locationToPlace = tilemap.CellToWorld(tilemapPos);

                // only add unit if there is not one there already
                if (!unitLocations.Contains(locationToPlace)) {
                    unitLocations.Add(locationToPlace);

                    // given location is bottom left corner, adjust for center of tile
                    locationToPlace += spriteOffset;
                    placedUnit = Instantiate(unitToPlace, locationToPlace, Quaternion.identity);
                    

                    directionSprites = placedUnit.transform.GetComponentsInChildren<SpriteRenderer>();
                
                    // start unit rotation
                    rotatingUnit = true;
                }
                
            }

        }
    }

    void handleRotationSelect() {
        // first, pause time so the user can select the direction
        Time.timeScale = 0;

        // show the arrow sprites attached to the unit
        renderDirectionArrows(true);

        // show the focus overlay centered on this unit
        renderFocusOverlay(true);

        // show the range overlay on the placed unit
        renderRangeOverlay(true);

        // listen for mouseclick hold for direction rotation display
        if (Input.GetMouseButton(0)) {
            Vector3 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);

            CardinalDirection spriteDirection = rotateUnit(clickPos);
        }

        // listen for mouseclick up on the arrows
        if (Input.GetMouseButtonUp(0)) {
            Vector3 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);

            CardinalDirection spriteDirection = rotateUnit(clickPos);

            if (spriteDirection != CardinalDirection.INVALID) {
                // rotation has been chosen, resume game
                renderDirectionArrows(false);
                renderFocusOverlay(false);
                renderRangeOverlay(false);

                rotatingUnit = false;
                Time.timeScale = 1;
            }
        }
    }

    CardinalDirection rotateUnit(Vector3 mousePos) {
        CardinalDirection directionToPlace = getDirectionClicked(mousePos);
        switch (directionToPlace) {
            case CardinalDirection.INVALID:
                break;
            case CardinalDirection.WEST:
                placedUnit.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case CardinalDirection.EAST:
                placedUnit.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case CardinalDirection.NORTH: 
                placedUnit.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case CardinalDirection.SOUTH:
                placedUnit.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
        return directionToPlace;
    }

    void renderDirectionArrows(bool enabled) {
        foreach (SpriteRenderer sprite in directionSprites) {
            if (sprite.tag == "DirectionArrow") {
                sprite.enabled = enabled;
            }
        }
    }

    void renderFocusOverlay(bool enabled) {
        focusOverlay.transform.position = placedUnit.transform.position;
        focusOverlaySprite.enabled = enabled;
    }

    void renderRangeOverlay(bool enabled) {
        foreach (SpriteRenderer sprite in directionSprites) {
            if (sprite.tag == "RangeOverlay") {
                sprite.enabled = enabled;
            }
        }
    }

    CardinalDirection getDirectionClicked(Vector3 clickPos) {
        Vector3 difference = clickPos - placedUnit.transform.position;

        if (difference.x > .5 && Mathf.Abs(difference.y) < 1) {
            //Debug.Log("rotating east");
            return CardinalDirection.EAST;
        } else if (difference.x < -.5 && Mathf.Abs(difference.y) < 1) {
            //Debug.Log("rotating west");
            return CardinalDirection.WEST;
        } else if (difference.y > .5 && Mathf.Abs(difference.x) < 1) {
            //Debug.Log("rotating north");
            return CardinalDirection.NORTH;
        } else if (difference.y < -.5 && Mathf.Abs(difference.x) < 1) {
            //Debug.Log("rotating south");
            return CardinalDirection.SOUTH;
        }

        return CardinalDirection.INVALID;
    }

    private enum CardinalDirection
    {
        NORTH, EAST, SOUTH, WEST, INVALID
    }
}