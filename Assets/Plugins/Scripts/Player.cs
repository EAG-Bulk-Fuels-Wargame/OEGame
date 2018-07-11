using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    string color;
    string userName;
    bool isTurn;
    bool playable;
    int actions;
    bool inAction;

    // Use this for initialization
    public Player() {
        color = "grey";
        userName = "observer";
        isTurn = false;
        playable = false;
        actions = 7;
        inAction = false;
    }

    public Player(string c, string un, bool it, bool p) {
        color = c;
        userName = un;
        isTurn = it;
        playable = p;
        actions = 7;
        inAction = false;

    }

	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isTurn) {
            RefreshActions();
        }
	}

    public bool GetIsTurn() {
        return isTurn;
    }

    public void SetIsTurn(bool it) {
        isTurn = it;
    }

    public void SetInAction(bool ia)
    {
        inAction = ia;
    }

    public void TakeAction() {
        actions--;
    }

    public void RefreshActions() {
        actions = 7;
    }

    public bool CanAct() {
        if (actions > 0 && isTurn && !inAction) {
            return true;
        }
        return false;
    }

}
