using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

    public Tile tile;
    public Transform dropContainer;
    public Transform berryContainer;
    public enum Direction {
        left,
        right,
        flip
    }

    List<Tile> dropTiles = new List<Tile>();
    List<Tile> stageTiles = new List<Tile>();
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
                RotateStage(Direction.flip);
            }
            if (Input.GetButtonDown("RotateLeft")) {
                RotateStage(Direction.left);
            }
            if (Input.GetButtonDown("RotateRight")) {
                RotateStage(Direction.right);
            }
        }
    }

    void RotateStage(Direction dir) {
        float currentRotation = berryContainer.rotation.eulerAngles.z;
        if (dir == Direction.flip) {
            LeanTween.scaleX(berryContainer.gameObject, berryContainer.localScale.x * -1, 1f).setEase(LeanTweenType.easeInOutQuad);
            foreach (Tile tile in stageTiles) {
                LeanTween.scaleY(tile.gameObject, tile.transform.localScale.x * -1, 1f).setEase(LeanTweenType.easeInOutElastic);
            }
        } else {
            if (dir == Direction.left) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation + 90, 1f).setEase(LeanTweenType.easeInOutQuad);
            } else if (dir == Direction.right) {
                LeanTween.rotateZ(berryContainer.gameObject, currentRotation - 90, 1f).setEase(LeanTweenType.easeInOutQuad);
            }
            foreach (Tile tile in stageTiles) {
                LeanTween.rotateZ(tile.gameObject, 0, 1f).setEase(LeanTweenType.easeInOutElastic);
            }
        }
    }

    void FillDropTiles(int amount) {

        for (int i = 0; i < amount; i++) {
            Tile newTile = tile.Spawn(new Vector3(i, 5, 0), Quaternion.identity);
            newTile.transform.parent = dropContainer;
            dropTiles.Add(newTile);
        }
    }

    void Drop() {
        foreach (Tile tile in dropTiles) {
            tile.transform.parent = berryContainer;
            stageTiles.Add(tile);
            LeanTween.moveY(tile.gameObject, 0, 1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(DropComplete);
        }
        dropTiles.Clear();
    }

    void DropComplete() {
        inputReady = true;
    }

    void CheckMatch() {

    }
}
