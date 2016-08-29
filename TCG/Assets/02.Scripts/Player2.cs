using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{

    public static int gold;                   //골드량
    public static int mana;
    public static int cp;
    public static int population;

    private Camera Camera2;                   //카메라에서 마우스 방향으로 Ray를 쏘기 위한 기준 카메라
    private Camera Camera1;

    private Material selected;                 //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal
    private Material normal;                   //material 원본
    private Material summonArea;


    private GameObject unitArcher;
    private GameObject instantUnit;

    private GameObject back;                  //바뀐 material을 돌려놓기 위한 gameobject
    private GameObject[] summonAreaTile;

    private bool mouseUp = false;
    private bool mouseclicked = false;
    private bool enterFlg = false;            //
    private bool materialflg = false;
    private int flgBuyUnits = 0;              //유닛 구매 flag
    public static bool selectCtrl = false;    //'Select Control' 버튼 표시

    RaycastHit rayObj;                        //ray에 닿은 오브젝트의 정보


    private string groundName;

    void Awake()
    {
        Camera2 = GameObject.Find("Camera2").GetComponent<Camera>() as Camera;
        Camera1 = GameObject.Find("Camera1").GetComponent<Camera>() as Camera;

        summonAreaTile = GameObject.FindGameObjectsWithTag("summon2");
    }

    void Start()
    {
        selected = system.selected;
        normal = system.normal;
        summonArea = system.summonArea;
        unitArcher = system.archer;

        gold = 1000;
        mana = 3;
        cp = 5;
        population = 0;

    }

    void Update()
    {

        if (flgBuyUnits == 2)                                                                      //유닛배치 flag 2
        {

            if (materialflg == false)
            {

                foreach (GameObject a in summonAreaTile)                                                //유닛배치 가능 구역 표시
                {
                    if (a.tag != "summon2")
                        continue;
                    a.GetComponent<MeshRenderer>().material = summonArea;
                }
                materialflg = true;
            }


            if (Input.GetButton("Cancel"))                                                         //유닛 배치 취소
            {
                foreach (GameObject a in summonAreaTile)
                {
                    a.GetComponent<MeshRenderer>().material = normal;
                }
                flgBuyUnits = 0;
            }

            Ray ray = Camera2.ScreenPointToRay(Input.mousePosition);                               //카메라에서 마우스 방향으로 ray발사

            if (Physics.Raycast(ray, out rayObj, 50)
                && rayObj.transform.tag == "summon2"
                && rayObj.transform.GetComponent<MeshRenderer>().material.color == summonArea.color)  //ray에 닿은 타일의 material이 소환영역이면 true
            {                                                                                         //즉, material이 바뀔때 마다 한번씩만 호출된다.
                if (enterFlg == true)
                    back.GetComponent<MeshRenderer>().material = summonArea;  //기존에 ray에 닿았던 타일의 material을 복구
                back = rayObj.transform.gameObject;                           //ray에 닿은 오브젝트를 back에 할당
                back.GetComponent<MeshRenderer>().material = selected;        //ray에 닿은 오브젝트의 material을 변경
                enterFlg = true;



            }

            if (Input.GetMouseButton(0))                              //배치하고 싶은 지형을 좌클릭
            {
                instantUnit = Instantiate(unitArcher, back.transform.position + Vector3.up, Quaternion.identity) as GameObject;
                instantUnit.tag = "Unit2";

                groundName = back.name;

                (instantUnit.GetComponent<playArcher>() as playArcher).coorR = GetR(groundName);
                (instantUnit.GetComponent<playArcher>() as playArcher).coorRU = GetRU(groundName);

                system.notMovable[system.unitCount, 0] = int.Parse(groundName.Substring(7, 1));
                system.notMovable[system.unitCount, 1] = int.Parse(groundName.Substring(9, 1));
                (instantUnit.GetComponent<playArcher>() as playArcher).unitCountNum = system.unitCount;

                system.unitCount++;

                gold -= 200;
                flgBuyUnits = 0;
                enterFlg = false;
                materialflg = false;

                foreach (GameObject a in summonAreaTile)                                                //유닛배치가 완료되면 타일 material을 원래대로 바꿈
                {
                    a.GetComponent<MeshRenderer>().material = normal;
                }
            }
        }




        if (selectCtrl == true)                                                                            //컨트롤할 유닛 선택
        {
            Ray rayUnitCtrl = Camera2.ScreenPointToRay(Input.mousePosition);                              //카메라에서 마우스 방향으로 ray발사

            if (Physics.Raycast(rayUnitCtrl, out rayObj, 50)
                 && rayObj.transform.tag == "Unit2")
            {

                if (Input.GetMouseButtonDown(0))
                {
                    mouseclicked = true;


                    if ((rayObj.transform.GetComponent("playArcher") as playArcher).ctrlSelected == false
                    && mouseUp == false)
                    {
                        (rayObj.transform.GetComponent("playArcher") as playArcher).ctrlSelected = true;
                        cp -= playArcher.control;
                    }


                    else if ((rayObj.transform.GetComponent("playArcher") as playArcher).ctrlSelected == true
                        && mouseUp == true)
                    {
                        (rayObj.transform.GetComponent("playArcher") as playArcher).ctrlSelected = false;
                        cp += playArcher.control;
                    }
                }
            }

            if (mouseclicked == true)
            {
                if (Input.GetMouseButtonUp(0)
                    && mouseUp == false)
                {
                    mouseUp = true;
                    mouseclicked = false;
                }
                else if (Input.GetMouseButtonUp(0)
                    && mouseUp == true)
                {
                    mouseUp = false;
                    mouseclicked = false;
                }
            }
        }




        if (selectCtrl == false && flgBuyUnits == 0)                                                       //Unit UI표시 기능
        {
            Ray rayUnitUi = Camera2.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayUnitUi, out rayObj, 50)
                && rayObj.transform.tag == "Unit2"
                && Input.GetMouseButton(0)
                && (rayObj.transform.GetComponent("playArcher") as playArcher).ctrlSelected == true)
            {
                (rayObj.transform.GetComponent("playArcher") as playArcher).unitUIOn = true;
            }
        }
    }

    void OnGUI()
    {

        GUI.Label(new Rect(10, 5, 100, 20), "Gold : " + gold.ToString());             //골드량 상단 좌측에 표시
        GUI.Label(new Rect(10, 20, 100, 20), "mana : " + mana.ToString());
        GUI.Label(new Rect(10, 35, 100, 20), "CP : " + cp.ToString());
        GUI.Label(new Rect(10, 50, 100, 20), "Population : " + population.ToString() + "/20");

        if (GUI.Button(new Rect(Screen.width - 210, Screen.height - 50, 200, 40), "Turn Over"))
        {
            Camera2.gameObject.SetActive(false);
            Camera1.gameObject.SetActive(true);
            (this.gameObject.GetComponent<Player2>() as Player2).enabled = false;
            (this.gameObject.GetComponent<Player1>() as Player1).enabled = true;

            system.whoseTurn = true;
        }

        if (selectCtrl == false)
        {
            if (GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Select Control")
                && flgBuyUnits == 0)
            {
                selectCtrl = true;
            }
        }
        else if (selectCtrl == true)
        {
            if (GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Selecting Finish"))
            {
                selectCtrl = false;
            }
        }

        if (flgBuyUnits == 0)                                                           //유닛 구매 버튼 표시 flag 0
        {
            if (GUI.Button(new Rect(10, Screen.height - 50, 200, 40), "Buy Units")      //유닛 구매 버튼 표시
                 && selectCtrl == false)
                flgBuyUnits++;                                                              //버튼 클릭시 다음 flag로
        }

        if (flgBuyUnits == 1)                                                           //유닛 구매 목록 표시 flag 1
        {
            GUI.Box(new Rect(5, Screen.height - 285, 110, 280), "Units");               //유닛 목록 표시 메뉴 틀
            if (GUI.Button(new Rect(10, Screen.height - 30, 100, 20), "Archer : 200G"))  //아처 구매 버튼
            {
                flgBuyUnits++;                                                          //유닛 배치 flag로
            }
        }

        if (flgBuyUnits == 2)
        {
            GUI.Box(new Rect(10, Screen.height - 30, 100, 20), "Cancel : ESC");        //취소 메세지
        }


    }

    private int GetR(string groundName)
    {
        if (groundName.Substring(8, 1) == ",")
        {
            return int.Parse(groundName.Substring(7, 1));
        }
        else if (groundName.Substring(9, 1) == ",")
        {
            return int.Parse(groundName.Substring(7, 2));
        }
        else
            return 0;
    }

    private int GetRU(string groundName)
    {
        if (groundName.Substring(8, 1) == ",")
        {
            if (groundName.Substring(10, 1) == ")")
            {
                return int.Parse(groundName.Substring(9, 1));
            }
            else if (groundName.Substring(11, 1) == ")")
            {
                return int.Parse(groundName.Substring(9, 2));
            }
            else
                return 0;
        }
        else if (groundName.Substring(9, 1) == ",")
        {
            if (groundName.Substring(11, 1) == ")")
            {
                return int.Parse(groundName.Substring(10, 1));
            }
            else if (groundName.Substring(12, 1) == ")")
            {
                return int.Parse(groundName.Substring(10, 2));
            }
            else
                return 0;
        }
        else
            return 0;
    }
}
