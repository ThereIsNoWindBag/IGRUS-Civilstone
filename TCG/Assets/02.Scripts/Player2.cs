using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Game.getInstance().getStatus() == Game.Status.P2) {

        }
    }

    void OnGUI() {

        if (GUI.Button(new Rect(Screen.width - 210, Screen.height - 50, 200, 40), "Turn Over")) {
            Game.getInstance().setStatus(Game.Status.P1);
        }
    }
}