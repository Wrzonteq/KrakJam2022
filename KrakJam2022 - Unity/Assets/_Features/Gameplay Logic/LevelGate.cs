using UnityEngine;

public class LevelGate : MonoBehaviour {
    [SerializeField] Collider2D entranceTrigger;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openedGateSprite;
    [SerializeField] Sprite closedGateSprite;


    public void Close() {
        spriteRenderer.sprite = closedGateSprite;
        entranceTrigger.enabled = false;
    }

    public void Open() {
        spriteRenderer.sprite = openedGateSprite;
        entranceTrigger.enabled = true;
    }
}
