using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarScript : MonoBehaviour
{

    private HexCell currentHex;
    public Text[] Words;

    private void Update()
    {
        //Creates text size and location
        Image s = GameObject.FindWithTag("TopBar").GetComponent<Image>();
        Words = s.GetComponentsInChildren<Text>();
        s.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 32);
        s.rectTransform.position = new Vector2(0, Screen.height);
        float height = s.rectTransform.sizeDelta.y;
        float width = s.rectTransform.sizeDelta.x;
        //Words[0].fontSize = 16;
        //Words[2].fontSize = 16;
        //if ((Words[0].preferredWidth) * 1.4 < width / 2)
        //{
        //    Words[0].fontSize += 2;
        //}
        //if (Words[0].preferredWidth > width / 16)
        //{
        //    Words[0].fontSize -= 2;
        //}
        //if ((Words[2].preferredWidth) * 1.4 < width / 2)
        //{
        //    Words[2].fontSize += 2;
        //}
        //if (Words[2].preferredWidth > width / 16)
        //{
        //    Words[2].fontSize -= 2;
        //}
    }
}
