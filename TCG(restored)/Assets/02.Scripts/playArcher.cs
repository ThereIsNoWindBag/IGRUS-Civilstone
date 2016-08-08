using UnityEngine;
using System.Collections;

public class playArcher : MonoBehaviour
{
    public static int unitNum = 0;              //유닛 번호
    public static int unitCountNum;

    public static int attack;
    public static int hp;
    public static int speed;
    public static int control;
    public static int range;

    private int leftSpeed;

    public bool ctrlSelected = false;    //컨트롤 권한 여부

    private GameObject arrowObj;                //Arrow 캔버스를 오브젝트로 담을 변수
    private GameObject unitUIObj;               //unitUI 캔버스를 오브젝트로 담을 변수
    private GameObject unitModel;               //유닛의 모델
    private GameObject[][] movableTile;

    private Material selectedTile;
    private Material summonTile;
    private Material normalTile;

    public bool unitUIOn = false;        //unitUI 표시 여부

    private Animator UIpop;

    public int coorR;
    public int coorRU;

    private int[,,] movable;
    private int[,] Temp;

    public int whatButton = 0;



    void Awake ()
    {
        attack  = unitsInfo.attack[unitNum];
        hp      = unitsInfo.hp[unitNum];
        speed   = unitsInfo.speed[unitNum];
        control = unitsInfo.control[unitNum];
        range   = unitsInfo.range[unitNum];

        leftSpeed = speed;

        coorR = 0;
        coorRU = 0;

        unitModel = GetComponent<Transform>().FindChild("modelObj").gameObject;
        unitModel.transform.rotation = Quaternion.Euler(0, 60, 0);

        arrowObj = GetComponent<Transform>().FindChild("Arrow").gameObject;   //오브젝트 하위의 Arrow캔버스를 찾는다
        //unitUIObj = GetComponent<Transform>().FindChild("/Archer Girl/unitUI/UIPanel").gameObject; 
        unitUIObj = GetComponent<Transform>().FindChild("unitUI").gameObject   //오브젝트 두단계 하위의 UIPanel을 찾는다
            .GetComponent<Transform>().FindChild("UIPanel").gameObject;

        //UIpop = GetComponent<Transform>().FindChild("/Archer Girl/unitUI/UIPanel").gameObject.GetComponent<Animator>(); 
        UIpop = GetComponent<Transform>().FindChild("unitUI").gameObject
            .GetComponent<Transform>().FindChild("UIPanel").gameObject.GetComponent<Animator>();//UIPanel에서 Animator컴포넌트를 찾아온다

        selectedTile = (GameObject.Find("script").GetComponent("Player1") as Player1).selected;
        summonTile = (GameObject.Find("script").GetComponent("Player1") as Player1).summonArea;
        normalTile = (GameObject.Find("script").GetComponent("Player1") as Player1).normal;

        movableTile = new GameObject[10][];

        movable = new int[10, 54, 2];
        Temp = new int[100, 2];

        
    }

	void Start ()
    {
        
	}
	

	void Update ()
    {

        if(ctrlSelected == true)                           //컨트롤 권한을 획득
        {
            arrowObj.SetActive(true);                      //화살표 나타나게하는거

            if (unitUIOn == true 
                && Player1.selectCtrl == false
                && Player1.uiOn == false)
            {
                unitUIObj.SetActive(true);
                UIpop.SetBool("click", true);
                Player1.uiOn = true;
            }
        }
        else if(ctrlSelected == false)                    //컨트롤 권한을 해제
        {
            arrowObj.SetActive(false);                    //화살표 사라지게함
        }

        
        if(whatButton == 2)
        {
            Debug.Log(coorR + "," + coorRU);
            findMovable(leftSpeed, coorR, coorRU);

                for (int mvrange = 1; mvrange <= leftSpeed; mvrange++)
                {

                    movableTile[mvrange - 1] = new GameObject[movable[mvrange, 0, 0]];

                    for (int layer = 1; layer <= movable[mvrange, 0, 0]; layer++)
                    {

                        int notExist = 0;

                        if (GameObject.Find("ground(" + movable[mvrange, layer, 0] + "," + movable[mvrange, layer, 1] + ")") == null)
                        {
                            notExist++;
                        }
                        else if (GameObject.Find("ground(" + movable[mvrange, layer, 0] + "," + movable[mvrange, layer, 1] + ")") != null)
                        {
                            movableTile[mvrange - 1][layer - 1 - notExist] =
                            GameObject.Find("ground(" + movable[mvrange, layer, 0] + "," + movable[mvrange, layer, 1] + ")");
                        }

                    }
                }

                foreach (GameObject[] dist in movableTile)
                {
                    if (dist == null)
                    {
                        break;
                    }
                    foreach (GameObject tile in dist)
                    {
                        if (tile == null)
                        {
                            break;
                        }
                        tile.GetComponent<MeshRenderer>().material = selectedTile;

                    }
                }

            whatButton = 0;
        }


        if (UIMgr.unitUIBtn == 5)
        {
            UIpop.SetBool("click", false);
            if(UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"))
            {
                Player1.uiOn = false;
                unitUIObj.SetActive(false);
                unitUIOn = false;
                UIMgr.unitUIBtn = 0;
            }
        }

	
	}


    public void findMovable(int leftspd, int R, int RU)
    {
        movable[0, 0, 0] = 1;
        movable[0, 1, 0] = R;  movable[0, 1, 1] = RU;
        
        for (int i = 1; i <= leftspd; i++)
        {
            movable[i, 0, 0] = 0;
        }
        


        for (int dist = 1; dist <= leftspd; dist++)
        {
            for(int t = 1; t <= movable[dist - 1, 0, 0]; t++)
            {
                temp(dist, movable[dist - 1, t, 0], movable[dist - 1, t, 1]);
            }
        }
    }

    public void temp(int layer, int r, int ru)
    {
        Temp[0, 0] = r + 1; Temp[0, 1] = ru;
        Temp[1, 0] = r;     Temp[1, 1] = ru + 1;
        Temp[2, 0] = r - 1; Temp[2, 1] = ru + 1;
        Temp[3, 0] = r - 1; Temp[3, 1] = ru;
        Temp[4, 0] = r;     Temp[4, 1] = ru - 1;
        Temp[5, 0] = r + 1; Temp[5, 1] = ru - 1;

        bool find = false;


        for (int i = 0; i <= 5; i++)
        {
            for (int j = 1; j <= Player1.unitCount; j++)
            {
                if (Temp[i, 0] == Player1.notMovable[j - 1, 0]
                    && Temp[i, 1] == Player1.notMovable[j - 1, 1])
                {
                    Temp[i, 0] = 0; Temp[i, 1] = 0;
                    find = true;
                    break;
                }
            }

            if (find == false)
            {
                for (int m = 1; m <= movable[layer, 0, 0]; m++)
                {
                    if (Temp[i, 0] == movable[layer, m, 0]
                        && Temp[i, 1] == movable[layer, m, 1])
                    {
                        Temp[i, 0] = 0; Temp[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }

            if(find == false)
            {
                for (int k = 1; k <= movable[layer - 1, 0, 0]; k++)
                {
                    if (Temp[i, 0] == movable[layer - 1, k, 0]
                        && Temp[i, 1] == movable[layer - 1, k, 1])
                    {
                        Temp[i, 0] = 0; Temp[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }

            if(find == false && layer >= 2)
            {
                for (int l = 1; l <= movable[layer - 2, 0, 0]; l++)
                {
                    if (Temp[i, 0] == movable[layer - 2, l, 0]
                        && Temp[i, 1] == movable[layer - 2, l, 1])
                    {
                        Temp[i, 0] = 0; Temp[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }
        }

        int empty = 0;

        for (int n = 0; n <= 5; n++)
        {
            if (Temp[n, 0] != 0 || Temp[n, 1] != 0)
            {
                movable[layer, movable[layer, 0, 0] + n + 1 - empty, 0] = Temp[n, 0];
                movable[layer, movable[layer, 0, 0] + n + 1 - empty, 1] = Temp[n, 1];

            }
            else if (Temp[n, 0] == 0 && Temp[n, 1] == 0)
            {
                empty++;
            }
        }

        movable[layer, 0, 0] += 6 - empty;
    }

}
