using UnityEngine;
using System.Collections;

public class system : MonoBehaviour
{
    public Material selectedTile;
    public Material normalTile;
    public Material summmonTile;
    public Material attackTile;
    public GameObject unitArcher;

    public static Material selected;                 //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal *
    public static Material normal;                   //material 원본 *
    public static Material summonArea;
    public static Material attack;

    public static GameObject archer;             //*

    public static int[,] notMovable;         //*

    public static int unitCount = 0;         //*

    public static bool whoseTurn = true;  //Player1 : true , Player2 : false
    void Awake()
    {
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
	
	}
}
