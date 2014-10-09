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
    public float animationSpeed = .5f;

    List<Tile> dropTiles = new List<Tile>();
    List<Tile> stageTiles = new List<Tile>();
    int[,] grid = new int[5, 5];
    bool inputReady = false;
    
    int stageDirection = 0;
    int rotation = 90;


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

        if (dir == RotationDirection.flip) {
            LeanTween.scaleY(berryContainer.parent.gameObject, berryContainer.parent.localScale.y * -1, animationSpeed).setEase(LeanTweenType.easeInOutQuad);

            foreach (Tile tile in stageTiles) {
                LeanTween.scaleY(tile.gameObject, tile.transform.localScale.y * -1, animationSpeed).setEase(LeanTweenType.easeInOutElastic);
            }
        } else {

            int currentRotation = (int)berryContainer.localRotation.eulerAngles.z;
            if (dir == RotationDirection.left) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation + rotation * berryContainer.parent.localScale.y, animationSpeed).setEase(LeanTweenType.easeInOutQuad);
            } else if (dir == RotationDirection.right) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation - rotation * berryContainer.parent.localScale.y, animationSpeed).setEase(LeanTweenType.easeInOutQuad);
            }
            foreach (Tile tile in stageTiles) {
                LeanTween.rotateZ(tile.gameObject, 0, animationSpeed).setEase(LeanTweenType.easeInOutElastic);
            }
        }
        Invoke("Drop", animationSpeed + .1f);
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
            if (grid[(int)tile.transform.position.x, 4] == 0) {
                tile.transform.position = new Vector3(tile.transform.position.x, 4, 0);
                tile.transform.parent = berryContainer;
                grid[(int)tile.transform.position.x, 4] = (int)tile.type;
                droppedTiles.Add(tile);
                stageTiles.Add(tile);
            }
        }
        foreach (Tile tile in droppedTiles) {
            dropTiles.Remove(tile);
        }
        droppedTiles.Clear();
        stageTiles = stageTiles.OrderBy(tile => tile.transform.position.y).ToList();

        bool isDropping = false;

        foreach (Tile tile in stageTiles) {
            //if (tile.canDrop && tile.transform.position.y > 0) {
            //    isDropping = true;
            //    LeanTween.moveY(tile.gameObject, tile.transform.position.y - 1, animationSpeed).setEase(LeanTweenType.easeOutBounce);
            //}
            if (tile.canDrop) {
                isDropping = true;
            }
            tile.Drop();
        }
        if (!isDropping) {
            DropComplete();
        } else {            
            Invoke("DropComplete", animationSpeed + .1f);
        }
    }

    void DropComplete() {
        CheckMatch();
    }

    void CheckMatch() {
        FillDropTiles(2);
        inputReady = true;
    }
}
