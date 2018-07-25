using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : WifiDirectBase {
    public GameObject canvas;
    public GameObject buttonList;
    public GameObject addrButton;
    public GameObject addrPanel;
    public GameObject colorPanel;
    public GameObject send;
    public InputField input;
	// Adds listeners to the color sliders and calls the initialize script on the library
	void Start () {
        colorPanel.SetActive(false);
        addrPanel.SetActive(false);
        canvas.SetActive(false);
        send.GetComponent<Button>().onClick.AddListener(() => {
            base.sendMessage(input.text); //when send button is clicked, send text
        });
        base.initialize(this.gameObject.name);
    }
	//when the WifiDirect services is connected to the phone, begin broadcasting and discovering services
    public override void onServiceConnected() {
        canvas.SetActive(true);
        addrPanel.SetActive(true);
        Dictionary<string, string> record = new Dictionary<string, string>();
        record.Add("demo", "unity");
        base.broadcastService("hello", record);
        base.discoverServices();
    }
	//On finding a service, create a button with that service's address
    public override void onServiceFound(string addr) {
        GameObject newButton = Instantiate(addrButton);
        newButton.GetComponentInChildren<Text>().text = addr;
        newButton.transform.SetParent(buttonList.transform, false);
        newButton.GetComponent<Button>().onClick.AddListener(() => {
            this.makeConnection(addr);
        });
    }
	//When the button is clicked, connect to the service at its address
    private void makeConnection(string addr) {
        base.connectToService(addr);
    }
	//When connected, begin rendering the cube
    public override void onConnect() {
        addrPanel.SetActive(false);
        colorPanel.SetActive(true);
    }

	//When recieving a new message, parse the color and set it to the cube
    public override void onMessage(string message) {
        input.text = message;
    }
	//Kill Switch
    public override void onServiceDisconnected() {
        base.terminate();
        Application.Quit();
    }
	//Kill Switch
    public void OnApplicationPause(bool pause) {
        if(pause) {
            this.onServiceDisconnected();
        }
    }
}
