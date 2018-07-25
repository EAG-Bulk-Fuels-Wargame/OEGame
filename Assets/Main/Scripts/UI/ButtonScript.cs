using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public void Button_Onclick()
    {
        GameObject.Find("PopUpSystem").SendMessage("Submit");
    }

    public void Cancel_Onclick()
    {
        GameObject.Find("PopUpSystem").SendMessage("Cancel");
    }

}