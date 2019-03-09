using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleImageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image img = GetComponent<Image>();
        Vector2 size = img.sprite.textureRect.size;
        Debug.Log(size);
        Debug.Log(img.rectTransform.rect.size);
        //for (int i = 0; i < size.y; i++)
        //{
        //    for (int j = 0; j < size.x; j++)
        //    {
        //        if (img.sprite.texture.GetPixel(i, j) == Color.red)
        //            Debug.Log(i + " " + j);
        //    }
        //}

        Debug.Log(img.PixelAdjustPoint(new Vector2(233.0f, 208.0f)));
        Debug.Log(img.sprite);
        Debug.Log(img.sprite.pivot);


        //for (int i = 0; i < size.y; i++)
        //{
        //    for (int j = 0; j < size.x; j++)
        //    {
        //        if (img.sprite.texture.GetPixel(j, i) == Color.red)
        //        {
        //            Debug.Log("It ok");
        //        }
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.zero, 0.05f);
    }
}
