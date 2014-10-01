using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum Types {
        blaubeer,
        himbeer,
        stachelbeer
        
    }
    public Types type;

    bool onStage = false;
    Sprite[] graphics;
    SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
        graphics = Resources.LoadAll<Sprite>("Sprites");
        sRenderer = GetComponent<SpriteRenderer>();
        SetType();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetType() {
        int randColor = Random.Range(0, 3);
        if (randColor == 0) {
            type = Types.blaubeer;
            sRenderer.sprite = graphics[0];
        } else if (randColor == 1) {
            type = Types.himbeer;
            sRenderer.sprite = graphics[2];
        } else if (randColor == 2) {
            type = Types.stachelbeer;
            sRenderer.sprite = graphics[4];
        }
    }
}
