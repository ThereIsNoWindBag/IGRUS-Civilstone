using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {

    public int gold;        
    public int mana;
    public int cp;
    public int population;  //플레이어2에게 주어진 자원들

    void Awake()
    {
        enabled = false;

        gold = 1000;
        mana = 3;
        cp = 5;
        population = 0;
    }

    void OnEnable()
    {
        CameraMove.getInstance().transform.position = new Vector3(25.098f, 15, 6.144435f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Game.getInstance().getStatus() == Game.Status.P2) {

        }
    }

    void OnGUI() {

        GUI.Label(new Rect(10, 10, 100, 20), "Gold : " + gold.ToString());             //골드량 상단 좌측에 표시
        GUI.Label(new Rect(10, 30, 100, 20), "mana : " + mana.ToString());
        GUI.Label(new Rect(10, 50, 100, 20), "CP : " + cp.ToString());
        GUI.Label(new Rect(10, 70, 100, 20), "Population : " + population.ToString() + "/20");

        if (GUI.Button(new Rect(Screen.width - 210, Screen.height - 50, 200, 40), "Turn Over")) {
            enabled = false;
            Game.getInstance().setStatus(Game.Status.P1);
        }
    }
}