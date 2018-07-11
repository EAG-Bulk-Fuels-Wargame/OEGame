using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour
{
    //Attach this script to a Dropdown GameObject
    private Dropdown m_Dropdown;

    //This is the string that stores the current selection m_Text of the Dropdown
    private string m_Message;

    //This Text outputs the current selection to the screen
    public Text m_Text;

    //This is the index value of the Dropdown
    private int m_DropdownValue;

    private bool initSet = false;

    private void Start()
    {
        //Fetch the DropDown component from the GameObject
        //m_Dropdown = GetComponent<Dropdown>()  ;
        m_Dropdown = GameObject.FindWithTag("dropper").GetComponent<Dropdown>();
        //Output the first Dropdown index value
        //Debug.Log("Starting Dropdown Value : " + m_Dropdown.value);
        //Update();
    }

    private void Update()
    {
        //Keep the current index of the Dropdown in a variable
        //m_Dropdown = GameObject.FindWithTag("dropper").GetComponent<Dropdown>();
        m_DropdownValue = m_Dropdown.value;
        //Change the message to say the name of the current Dropdown selection using the value
        m_Message = m_Dropdown.options[m_DropdownValue].text;
        //Change the onscreen Text to reflect the current Dropdown selection
        //m_Text.text = m_Message;
        //Debug.Log("Dropdown Text: " + m_Message);
        if (!initSet)
        {
            GameObject.Find("PopUpSystem").SendMessage("Commit", m_Dropdown.options[0].text);
            initSet = true;
        }
    }

    public void WriteValue()
    {
        Start();
        Update();
        GameObject.Find("PopUpSystem").SendMessage("Commit", m_Message);
        //.GetScript().HexPopUp.Commit(m_Message);
    }
}