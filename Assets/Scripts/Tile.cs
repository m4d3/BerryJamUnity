using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum Types {
        empty,
        blaubeer,
        himbeer,
        stachelbeer
    }
    public Types type;

    public Sprite[] graphics;
    public bool canDrop = true;
    SpriteRenderer sRenderer;

    // Use this for initialization
    void Start() {
        graphics = Resources.LoadAll<Sprite>("Sprites");
    }

    void OnEnable() {
        SetType();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetType() {
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.position.y < transform.position.y) {
            canDrop = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (transform.position.y > 0)
            canDrop = true;
    }

    public void Drop() {
        if (canDrop && transform.position.y > 0) {
            LeanTween.moveY(gameObject, transform.position.y - 1, 0.5f).setEase(LeanTweenType.easeOutBounce).setOnComplete(Drop);
        }
    }
}
