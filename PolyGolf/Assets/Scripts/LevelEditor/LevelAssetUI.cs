using UnityEngine;
using UnityEngine.UI;

public class LevelAssetUI : MonoBehaviour
{
    public int assetId;

    public void ClickItem()
    {
        GameObject.FindGameObjectWithTag("PlacementSystem")
                  .GetComponent<AssetPlacementSystem>()
                  .StartPlacement(assetId);
    }


    public void SetSprite(Sprite newSprite)
    {
        Image spriteImage = transform.GetChild(1).GetComponent<Image>();
        spriteImage.sprite = newSprite;
    }
}
