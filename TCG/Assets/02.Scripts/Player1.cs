using UnityEngine;
using System.Collections;

public class Player1 : MonoBehaviour {

    public enum BuyStatus { Not, UnitSelect, TileSelect }; //@
    private BuyStatus buyStatus; //@플래그들은 enum 써서 사용해주세요

    public static int gold; //골드량
    public static int mana;
    public static int cp;
    public static int population; //플레이어1에게 주워진 자원들

    //Camera
    private Camera Camera1; //카메라에서 마우스 방향으로 Ray를 쏘기 위한 기준 카메라
    private Camera Camera2;

    //Material
    private Material selected;  //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal *
    private Material normal;                   //material 원본 *
    private Material summonArea;               //*

    private GameObject unitArcher;             //*
    private GameObject instantUnit;

    //(GameObject) Material
    private GameObject back;    //바뀐 material을 돌려놓기 위한 gameobject
    private GameObject[] summonAreaTile;

    //플래그들
    private bool mouseUp = false;
    private bool mouseclicked = false;
    private bool enterFlg = false;            
    private bool materialflg = false;
    public static bool selectCtrl = false;    //'Select Control' 버튼 표시 플래그

    //@Ray관련
    private Ray ray;    //카메라에서 마우스 방향으로 ray발사
    private RaycastHit rayObj;  //@ray에 닿은 오브젝트의 정보

    private string groundName;

    void Awake()
    {
        Camera2 = GameObject.Find("Camera2").GetComponent<Camera>() as Camera;
        Camera1 = GameObject.Find("Camera1").GetComponent<Camera>() as Camera;

        summonAreaTile = GameObject.FindGameObjectsWithTag("summon1");
    }

    void Start()
    {
        selected = Game.selected;
        normal = Game.normal;
        summonArea = Game.summonArea;
        unitArcher = Game.archer;

        gold = 1000;
        mana = 3;
        cp = 5;
        population = 0;
    }

    void OnEnable()
    {
        CameraMove.getInstance().transform.position = new Vector3(2.598f, 15, -5.821666f);
    }

    void Update ()
    {
        ray = Camera1.ScreenPointToRay(Input.mousePosition);

        //if (Game.getInstance().getStatus() == Game.Status.P1) {  //자신의 턴인지 확인
        if (Input.GetMouseButton(0))    //오른쪽 마우스 클릭
        {
            ray = Camera1.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out rayObj, 50);

            if (rayObj.transform.tag == "summon1")
            {
                //땅 눌렀을때
            }

            else if (rayObj.transform.tag == "Unit1")
            {
                //유닛 눌렀을때
            }

            else if (rayObj.transform.tag == "summon2")
            {
                //유닛 눌렀을때
            }

            else if (rayObj.transform.tag == "Unit2")
            {
                //유닛 눌렀을때
            }
        }

        else if (Input.GetMouseButton(1))   //왼쪽 마우스 클릭
        {

        }   //배치하고 싶은 지형을 좌클릭
        else if (Input.GetMouseButton(2))   //마우스 휠 클릭
        {

        }   //배치하고 싶은 지형을 좌클릭

        if (buyStatus == BuyStatus.TileSelect)  //타일 선택 상태
        {

            if (materialflg == false)
            {
                
                foreach (GameObject a in summonAreaTile)    //유닛배치 가능 구역 표시                                              
                {
                    if (a.tag != "summon1")
                        continue;
                    a.GetComponent<MeshRenderer>().material = summonArea;
                }
                materialflg = true;
            }

            if (Input.GetButton("Cancel"))  //유닛 배치 취소, 기본 대기 상태로 돌아감
            {
                foreach (GameObject a in summonAreaTile)
                {
                    a.GetComponent<MeshRenderer>().material = normal;
                }
                buyStatus = BuyStatus.Not;
            }

            if (Physics.Raycast(ray, out rayObj, 50) 
                && rayObj.transform.tag == "summon1"
                && rayObj.transform.GetComponent<MeshRenderer>().material.color == summonArea.color)  //ray에 닿은 타일의 material이 소환 가능 영역이면 괄호내 구문 실행
            {
                if (enterFlg == true)
                    back.GetComponent<MeshRenderer>().material = summonArea;  //기존에 ray에 닿았던 타일의 material을 복구
                back = rayObj.transform.gameObject;                           //ray에 닿은 오브젝트를 back에 할당
                back.GetComponent<MeshRenderer>().material = selected;        //ray에 닿은 오브젝트의 material을 변경
                enterFlg = true;
            }

            if (Input.GetMouseButton(0))                              //배치하고 싶은 지형을 좌클릭
            {
                instantUnit = Instantiate(unitArcher, back.transform.position + Vector3.up, Quaternion.identity) as GameObject;
                instantUnit.tag = "Unit1";

                groundName = back.name;

                (instantUnit.GetComponent<PlayArcher>() as PlayArcher).coorR = GetR(groundName);
                (instantUnit.GetComponent<PlayArcher>() as PlayArcher).coorRU = GetRU(groundName);

                Game.notMovable[Game.unitCount, 0] = int.Parse(groundName.Substring(7, 1));
                Game.notMovable[Game.unitCount, 1] = int.Parse(groundName.Substring(9, 1));
                (instantUnit.GetComponent<PlayArcher>() as PlayArcher).unitCountNum = Game.unitCount;

                Game.unitCount++;

                gold -= 200;
                buyStatus = BuyStatus.Not;
                enterFlg = false;
                materialflg = false;

                foreach (GameObject a in summonAreaTile)                                                //유닛배치가 완료되면 타일 material을 원래대로 바꿈
                {
                    a.GetComponent<MeshRenderer>().material = normal;
                }
            }
        }

            //여기다
            if(selectCtrl == true)  //컨트롤할 유닛 선택
            {
                if(Physics.Raycast(ray, out rayObj, 50)
                     && rayObj.transform.tag == "Unit1")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        mouseclicked = true;

                        if ((rayObj.transform.GetComponent("PlayArcher") as PlayArcher).ctrlSelected == false
                        && mouseUp == false)
                        {
                            (rayObj.transform.GetComponent("PlayArcher") as PlayArcher).ctrlSelected = true;
                            cp -= PlayArcher.control;
                        }

                        else if ((rayObj.transform.GetComponent("PlayArcher") as PlayArcher).ctrlSelected == true
                            && mouseUp == true)
                        {
                            (rayObj.transform.GetComponent("PlayArcher") as PlayArcher).ctrlSelected = false;
                            cp += PlayArcher.control;
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




            if(selectCtrl == false && buyStatus == BuyStatus.Not)   //Unit UI표시 기능
            {
                Ray rayUnitUi = Camera1.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(rayUnitUi, out rayObj, 50)
                    && rayObj.transform.tag == "Unit1"
                    && Input.GetMouseButton(0)
                    && (rayObj.transform.GetComponent("PlayArcher") as PlayArcher).ctrlSelected == true)
                {
                    (rayObj.transform.GetComponent("PlayArcher") as PlayArcher).unitUIOn = true;
                }
            }
        //} //@위의 자신 턴 확인 if문 괄호
    }

    void OnGUI()
    {
        //자원량 표시
        GUI.Label(new Rect(10, 5, 100, 20), "Gold : " + gold.ToString());            
        GUI.Label(new Rect(10, 20, 100, 20), "mana : " + mana.ToString());
        GUI.Label(new Rect(10, 35, 100, 20), "CP : " + cp.ToString());
        GUI.Label(new Rect(10, 50, 100, 20), "Population : " + population.ToString() + "/20");

        //턴 넘기기 버튼
        if(GUI.Button(new Rect(Screen.width -210, Screen.height - 50, 200, 40), "Turn Over"))
        {
            Camera1.gameObject.SetActive(false);
            Camera2.gameObject.SetActive(true);
            (this.gameObject.GetComponent<Player1>() as Player1).enabled = false;
            (this.gameObject.GetComponent<Player2>() as Player2).enabled = true;

            Game.whoseTurn = false;
        }
        
        //여기를 없애야 한다.
                                                                                                    if (selectCtrl == false)
                                                                                                    {
                                                                                                        if(GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Select Control") 
                                                                                                            && buyStatus == BuyStatus.Not)
                                                                                                        {
                                                                                                            selectCtrl = true;
                                                                                                        }
                                                                                                    }
                                                                                                    else if (selectCtrl == true)
                                                                                                    {
                                                                                                        if(GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Selecting Finish"))
                                                                                                        {
                                                                                                            selectCtrl = false;
                                                                                                        }
                                                                                                    }

        //상점
        if (buyStatus == BuyStatus.Not)                                                           //유닛 구매 버튼 표시 flag 0
        {
            if (GUI.Button(new Rect(10, Screen.height - 50, 200, 40), "Buy Units")      //유닛 구매 버튼 표시
                 && selectCtrl == false)
                buyStatus++;                                                              //버튼 클릭시 다음 flag로
        }

        if (buyStatus == BuyStatus.UnitSelect)                                                           //유닛 구매 목록 표시 flag 1
        {
            GUI.Box(new Rect(5, Screen.height - 285, 110, 280), "Units");               //유닛 목록 표시 메뉴 틀
            if(GUI.Button(new Rect(10, Screen.height - 30, 100 ,20), "Archer : 200G"))  //아처 구매 버튼
            {
                buyStatus++;                                                          //유닛 배치 flag로
            }
        }

        if (buyStatus == BuyStatus.TileSelect)
        {
            GUI.Box(new Rect(10, Screen.height - 30, 100, 20), "Cancel : ESC");        //취소 메세지
        }


    }

    //@이거 머임
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
