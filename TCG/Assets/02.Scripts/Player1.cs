using UnityEngine;
using System.Collections;

public class Player1 : MonoBehaviour {

    public int gold;
    public int mana;
    public int cp;
    public int population;      //플레이어1에게 주워진 자원들

    public Camera camera1;      //카메라에서 마우스 방향으로 Ray를 쏘기 위한 기준 카메라

    public Material selected;       //유닛 구매시 유닛을 올려놓을 자리를 표시하기 위한 materal
    public Material normal;     //material 원본
    public Material summonArea;

    public GameObject back;     //바뀐 material을 돌려놓기 위한 gameobject
    public GameObject unitArcher;

    private GameObject[] summonAreaTile;

    private bool mouseUp = false;
    private bool mouseclicked = false;

    private int flgBuyUnits = 0;              //유닛 구매 flag
    public static bool selectCtrl = false;    //'Select Control' 버튼 표시

    RaycastHit rayObj;                        //ray에 닿은 오브젝트의 정보

    void Awake ()
    {
        enabled = false;
 
        gold = 1000;
        mana = 3;
        cp = 5;
        population = 0;

        summonAreaTile = GameObject.FindGameObjectsWithTag("summon");
    }

    void OnEnable()
    {
        CameraMove.getInstance().transform.position = new Vector3(2.598f, 15, -5.821666f);
    }
	
	void Update ()
    {
        if (Game.getInstance().getStatus() == Game.Status.P1) {     //자신의 턴인지 확인

            if (flgBuyUnits == 2)                                                                      //유닛배치 flag 2
            {
                Ray ray = camera1.ScreenPointToRay(Input.mousePosition);                               //카메라에서 마우스 방향으로 ray발사

                foreach (GameObject a in summonAreaTile)                                                //유닛배치 가능 구역 표시
                {
                    a.GetComponent<MeshRenderer>().material = summonArea;
                }

                if (Input.GetButton("Cancel"))                                                         //유닛 배치 취소
                {
                    foreach (GameObject a in summonAreaTile) {
                        a.GetComponent<MeshRenderer>().material = normal;
                    }
                    flgBuyUnits = 0;
                }

                if (Physics.Raycast(ray, out rayObj, 50)
                    && rayObj.transform.GetComponent<MeshRenderer>().material.color == summonArea.color) //ray에 닿은 타일의 material이 소환영역이면 true
                {                                                                                        //즉, material이 바뀔때 마다 한번씩만 호출된다.

                    //Debug.Log("hit");
                    back.GetComponent<MeshRenderer>().material = summonArea;  //기존에 ray에 닿았던 타일의 material을 복구
                    back = rayObj.transform.gameObject;                      //ray에 닿은 오브젝트를 back에 할당
                    back.GetComponent<MeshRenderer>().material = selected;    //ray에 닿은 오브젝트의 material을 변경

                    if (Input.GetMouseButton(0)) //배치하고 싶은 지형을 좌클릭
                    {
                        Instantiate(unitArcher, rayObj.transform.position + Vector3.up, Quaternion.Euler(0, 60, 0));
                        unitArcher.tag = "Unit1";
                        gold -= 200;
                        flgBuyUnits = 0;

                        foreach (GameObject a in summonAreaTile)                                                //유닛배치가 완료되면 타일 material을 원래대로 바꿈
                        {
                            a.GetComponent<MeshRenderer>().material = normal;
                        }
                    }

                }
            }




            if (selectCtrl == true)                                                                            //컨트롤할 유닛 선택
            {
                Ray rayUnitCtrl = camera1.ScreenPointToRay(Input.mousePosition);                              //카메라에서 마우스 방향으로 ray발사

                if (Physics.Raycast(rayUnitCtrl, out rayObj, 50)
                     && rayObj.transform.tag == "Unit1") {

                    if (Input.GetMouseButtonDown(0)) {
                        mouseclicked = true;


                        if (playArcher.ctrlSelected == false
                        && mouseUp == false) {
                            playArcher.ctrlSelected = true;
                            cp -= playArcher.control;
                        }


                        else if (playArcher.ctrlSelected == true
                            && mouseUp == true) {
                            playArcher.ctrlSelected = false;
                            cp += playArcher.control;
                        }
                    }
                }

                if (mouseclicked == true) {
                    if (Input.GetMouseButtonUp(0)
                        && mouseUp == false) {
                        mouseUp = true;
                        mouseclicked = false;
                    }
                    else if (Input.GetMouseButtonUp(0)
                        && mouseUp == true) {
                        mouseUp = false;
                        mouseclicked = false;
                    }
                }
            }




            if (selectCtrl == false && flgBuyUnits == 0)                                                       //Unit UI표시 기능
            {
                Ray rayUnitUi = camera1.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(rayUnitUi, out rayObj, 50)
                    && rayObj.transform.tag == "Unit1"
                    && Input.GetMouseButton(0)) {
                    playArcher.unitUIOn = true;
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

        if(GUI.Button(new Rect(Screen.width -210, Screen.height - 50, 200, 40), "Turn Over")) { //턴 종료 버튼
            enabled = false;
            Game.getInstance().setStatus(Game.Status.P2);
        }

        if(selectCtrl == false)
        {
            if (GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Select Control")
                && flgBuyUnits == 0) {
                selectCtrl = true;
            }
        }
        else if (selectCtrl == true) {
            if (GUI.Button(new Rect(220, Screen.height - 50, 200, 40), "Selecting Finish")) {
                selectCtrl = false;
            }
        }

        if (flgBuyUnits == 0)                                                           //유닛 구매 버튼 표시 flag 0
        {
            if(GUI.Button(new Rect(10, Screen.height - 50, 200, 40), "Buy Units"))     //유닛 구매 버튼 표시
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
