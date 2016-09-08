using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    //@ Game 추가본
    private static Game instance; //싱글톤 디자인

    //지형 머테리얼들
    public Material selectedTile;
    public Material normalTile;
    public Material summmonTile;
    public Material attackTile;
    public GameObject unitArcher;

    //차례 enum
    public enum Status { P1, P2, P1won, P2won }; //public으로 해줘야 함수 사용 가능
    private Status status;

    public bool paused; //

    private Player1 p1;
    private Player2 p2;
    //여기까지

    //뭐지
    public static Material selected;    //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal *
    public static Material normal;  //material 원본 *
    public static Material summonArea;
    public static Material attack;
    public static GameObject archer;

    public static int[,] notMovable;         //*

    public static int unitCount = 0;         //*

    public static bool whoseTurn = true;  //Player1 : true , Player2 : false

    public static Game getInstance() //싱글톤 디자인
    {
        return instance;
    }

    //게임 Status setter&getter
    public void setStatus(Status s)
    {
        status = s;
    }

    public Status getStatus()
    {
        return status;
    }

    void Awake()
    {
        instance = this;

        p1 = GetComponent<Player1>();
        p2 = GetComponent<Player2>();

        status = Status.P1;

        selected = selectedTile;
        normal = normalTile;
        summonArea = summmonTile;
        attack = attackTile;
        archer = unitArcher;

        notMovable = new int[20,2];
    }

	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        switch(status) {
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
