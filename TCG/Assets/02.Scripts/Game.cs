using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public static Game instance;

    public enum Status { P1, P2, P1won, P2won };
    private Status status;

    public bool paused;

    private Player1 p1;
    private Player2 p2;

    public static Game getInstance() {
        return instance;
    }

    public void setStatus(Status s) {
        status = s;
    }

    public Status getStatus() {
        return status;
    }

    // Use this for initialization
    public void Awake () {
        instance = this;

        p1 = GetComponent<Player1>();
        p2 = GetComponent<Player2>();

        status = Status.P1;
	}

	// Update is called once per frame
	void Update () {
        switch (status) {
            case Status.P1:
                p1.enabled = true;
                break;
            case Status.P2:
                p2.enabled = true;
                break;
            case Status.P1won:
                break;
            case Status.P2won:
                break;
        }
        
        //P1일때 Player1 활성화.
        //P2일때 Player2 활성화.
        //Pause일때 게임 멈추기.
	
	}
}