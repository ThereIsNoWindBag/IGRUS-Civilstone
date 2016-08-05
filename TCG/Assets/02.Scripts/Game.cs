using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public enum Status { P1, P2, Pause, P1won, P2won };
    public static Game instance;

    private Status status;

    public static Game getInstance() {
        return instance;
    }

    // Use this for initialization
    void Start () {
        game = this;
        status = Status.P1;
	}
	
	// Update is called once per frame
	void Update () {

        //P1일때 Player1 활성화.
        //P2일때 Player2 활성화.
        //Pause일때 게임 멈추기.
	
	}
}
