using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public enum Status { P1, P2, Pause, P1won, P2won };
    public static Game instance;

    private Status status;

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
    void Start () {
        instance = this;
        status = Status.P1;
	}

	// Update is called once per frame
	void Update () {
        switch (status) {
            case Status.P1:
                break;
            case Status.P2:
                break;
            case Status.Pause:
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