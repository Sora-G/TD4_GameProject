using UnityEngine;
using UnityEngine.UI;

public class CoreSlot : MonoBehaviour
{
    public int x;
    public int y;
    public bool isOccupied = false; // 💡 このマスにパーツが置かれているか
    public CoreItemUI placedItem = null; // 💡 置かれているパーツのデータ

    private Image myImage;

    void Awake()
    {
        myImage = GetComponent<Image>();
    }

    // 💡 マスの見た目（デバッグ用などに色を変えられるようにしておく）
    public void SetHighlight(bool highlight, Color color)
    {
        if (myImage != null && !isOccupied)
        {
            myImage.color = highlight ? color : new Color(1, 1, 1, 0.1f); // 通常時はうっすら白い半透明
        }
    }
}