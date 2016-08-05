using UnityEngine;
using System.Collections;

public class Player1 : MonoBehaviour {

    public int gold;        //골드량
    public int mana;
    public int cp;
    public int population;

    public Camera camera1;          //카메라에서 마우스 방향으로 Ray를 쏘기 위한 기준 카메라

    public Material selected;       //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal
    public Material normal;         //material 원본
    public Material summonArea;

    public GameObject back;         //바뀐 material을 돌려놓기 위한 gameobject
    public GameObject unitArcher;

    private GameObject[] summonAreaTile;

    private int flgBuyUnits = 0;    //유닛 구매 flag

    private void Awake ()
    {
        summonAreaTile = GameObject.FindGameObjectsWithTag("summon");
    }

	void Start ()
    {
        gold = 1000;
        mana = 3;
        cp = 5;
        population = 0;
    }
	
	void Update ()
    {
        Ray ray = camera1.ScreenPointToRay(Input.mousePosition);                                   //카메라에서 마우스 방향으로 ray발사

        RaycastHit objTile;                                                                        // ray에 닿은 물체의 정보

        if (flgBuyUnits == 2)                                                                      //유닛배치 flag 2
        {
           
            foreach(GameObject a in summonAreaTile)                                                //유닛배치 가능 구역 표시
            {
                a.GetComponent<MeshRenderer>().material = summonArea;
            }

            if (Input.GetButton("Cancel"))                                                         //유닛 배치 취소
            {
                foreach (GameObject a in summonAreaTile)
                {
                    a.GetComponent<MeshRenderer>().material = normal;
                }
                flgBuyUnits = 0;
            }

            if (Physics.Raycast(ray, out objTile, 100) 
                && objTile.transform.GetComponent<MeshRenderer>().material.color == summonArea.color) //ray에 닿은 타일의 material이 소환영역이면 true
            {                                                                                         //즉, material이 바뀔때 마다 한번씩만 호출된다.

                //Debug.Log("hit");
                back.GetComponent<MeshRenderer>().material = summonArea; //기존에 ray에 닿았던 타일의 material을 복구
                back = objTile.transform.gameObject;                      //ray에 닿은 오브젝트를 back에 할당
                back.GetComponent<MeshRenderer>().material = selected;    //ray에 닿은 오브젝트의 material을 변경

                if (Input.GetMouseButton(0)) //배치하고 싶은 지형을 좌클릭
                {
                    Instantiate(unitArcher, objTile.transform.position + Vector3.up, Quaternion.Euler(0, 60, 0));
                    gold -= 200;
                    flgBuyUnits = 0;

                    foreach (GameObject a in summonAreaTile)                                                //유닛배치가 완료되면 타일 material을 원래대로 바꿈
                    {
                        a.GetComponent<MeshRenderer>().material = normal;
                    }
                }

            } 
        }
	}

    void OnGUI()
    {

        GUI.Label(new Rect(10, 10, 100, 20), "Gold : " + gold.ToString());             //골드량 상단 좌측에 표시
        GUI.Label(new Rect(10, 30, 100, 20), "mana : " + mana.ToString());
        GUI.Label(new Rect(10, 50, 100, 20), "CP : " + cp.ToString());
        GUI.Label(new Rect(10, 70, 100, 20), "Population : " + population.ToString() + "/20");

        if(GUI.Button(new Rect(Screen.width -210, Screen.height - 50, 200, 40), "Turn Over"))
        {

        }

        if(flgBuyUnits == 0)                                                           //유닛 구매 버튼 표시 flag 0
        {
            if(GUI.Button(new Rect(10, Screen.height - 30, 200, 40), "Buy Units"))     //유닛 구매 버튼 표시
            flgBuyUnits++;                                                             //버튼 클릭시 다음 flag로
        }

        if(flgBuyUnits == 1)                                                           //유닛 구매 목록 표시 flag 1
        {
            GUI.Box(new Rect(5, Screen.height - 285, 110, 280), "Units");              //유닛 목록 표시 메뉴 틀
            if(GUI.Button(new Rect(10, Screen.height - 30, 100 ,20), "Archer : 200G")) //아처 구매 버튼
            {
                flgBuyUnits++;                                                         //유닛 배치 flag로
            }
        }

        if(flgBuyUnits == 2)
        {
            GUI.Box(new Rect(10, Screen.height - 30, 100, 20), "Cancel : ESC");        //취소 메세지
        }


    }
}
