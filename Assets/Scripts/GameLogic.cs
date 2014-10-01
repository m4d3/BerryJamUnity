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
        }
    }

    void RotateStage(Direction dir) {
        if (dir == Direction.flip) {
            LeanTween.rotateZ(berryContainer.gameObject, 180, 1f).setEase(LeanTweenType.easeInOutElastic);
            foreach (Tile tile in stageTiles) {
                tile.transform.parent = berryContainer;
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
}
