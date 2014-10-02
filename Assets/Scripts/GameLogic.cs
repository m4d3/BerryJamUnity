using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameLogic : MonoBehaviour {

    public Tile tile;
    public Transform dropContainer;
    public Transform berryContainer;
    public enum RotationDirection {
        left,
        right,
        flip
    }
    public enum StageDirection
    {
        up,
        down,
        left,
        right
    }

    StageDirection direction = StageDirection.down;
    List<Tile> dropTiles = new List<Tile>();
    List<Tile> stageTiles = new List<Tile>();
    int[,] grid = new int[5,5];
    bool inputReady = false;


    // Use this for initialization
    void Start() {
        tile.CreatePool(35);
        FillDropTiles(5);
        Drop();
    }

    // Update is called once per frame
    void Update() {
        if (inputReady) {
            if (Input.GetButtonDown("Flip")) {
                RotateStage(RotationDirection.flip);
            }
            if (Input.GetButtonDown("RotateLeft")) {
                RotateStage(RotationDirection.left);
            }
            if (Input.GetButtonDown("RotateRight")) {
                RotateStage(RotationDirection.right);
            }
        }
    }

    void RotateStage(RotationDirection dir) {
        inputReady = false;
        float currentRotation = berryContainer.rotation.eulerAngles.z;
        if (dir == RotationDirection.flip) {
            LeanTween.scaleX(berryContainer.gameObject, berryContainer.localScale.x * -1, 1f).setEase(LeanTweenType.easeInOutQuad);
            foreach (Tile tile in stageTiles) {
                LeanTween.scaleY(tile.gameObject, tile.transform.localScale.x * -1, 1f).setEase(LeanTweenType.easeInOutElastic);
            }
        } else {
            if (dir == RotationDirection.left) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation + 90, 1f).setEase(LeanTweenType.easeInOutQuad);
            } else if (dir == RotationDirection.right) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation - 90, 1f).setEase(LeanTweenType.easeInOutQuad);
            }
            foreach (Tile tile in stageTiles) {
                LeanTween.rotateZ(tile.gameObject, 0, 1f).setEase(LeanTweenType.easeInOutElastic);
            }
        }
        Invoke("Drop", 1.1f);
    }

    void FillDropTiles(int amount) {

        for (int i = 0; i < amount; i++) {
            Tile newTile = tile.Spawn(new Vector3(i, 5, 0), Quaternion.identity);
            newTile.transform.parent = dropContainer;
            dropTiles.Add(newTile);
        }
    }

    void Drop() {
        List<Tile> droppedTiles = new List<Tile>();
        foreach (Tile tile in dropTiles) {
            if (grid[(int)tile.transform.position.x, 0] == 0)
            {
                tile.transform.position = new Vector3(tile.transform.position.x, 4, 0);
                tile.transform.parent = berryContainer;
                grid[(int)tile.transform.position.x, 4] = (int)tile.type;
                droppedTiles.Add(tile);
                stageTiles.Add(tile);
            }            
        }
        foreach(Tile tile in droppedTiles) {
            dropTiles.Remove(tile);
        }
        droppedTiles.Clear();
        stageTiles = stageTiles.OrderBy(tile => tile.transform.position.y ).ToList();

        bool isDropping = false;

        foreach (Tile tile in stageTiles)
        {
            int newPosition = -1;
            if (tile.transform.position.y != 0)
            {
                for (int y = (int)tile.transform.position.y; y > -1; --y)
                {
                    if (grid[(int)tile.transform.position.x, y] == 0)
                    {
                        newPosition = y;
                    }
                }
                if (newPosition != -1)
                {
                    isDropping = true;
                    grid[(int)tile.transform.position.x, (int)tile.transform.position.y] = 0;
                    grid[(int)tile.transform.position.x, newPosition] = (int)tile.type;
                    LeanTween.moveY(tile.gameObject, newPosition, 1f).setEase(LeanTweenType.easeOutBounce);
                }
            }
        }
        if (!isDropping)
        {
            DropComplete();
        }
        else
        {
            Invoke("DropComplete", 1.1f);
        }
    }

    void DropComplete() {        
        CheckMatch();
    }

    void CheckMatch() {
        Debug.Log("Checking");
        FillDropTiles(2);
        inputReady = true;
    }
}
