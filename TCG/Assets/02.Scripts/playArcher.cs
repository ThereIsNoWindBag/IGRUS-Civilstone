using UnityEngine;
using System.Collections;

public class playArcher : MonoBehaviour
{
    public static int unitNum = 0;              //유닛 번호
    public int unitCountNum;

    public static int attack;
    public static int hp;
    public static int speed;
    public static int control;
    public static int range;

    private int leftSpeed;
    private int leftHp;

    public Camera came;

    public bool ctrlSelected = false;    //컨트롤 권한 여부
    private bool enterFlg = false;       //Update()함수 안의 if(whatButton == 2) 내부에서 사용
    private bool tag = false;
    public bool unitUIOn = false;        //unitUI 표시 여부
    private bool materialflg = true;

    private GameObject arrowObj;                //Arrow 캔버스를 오브젝트로 담을 변수
    private GameObject unitUIObj;               //unitUI 캔버스를 오브젝트로 담을 변수
    private GameObject unitModel;               //유닛의 모델
    private GameObject[][] movableTile;
    private GameObject back;                   //바뀐 material을 돌려놓기 위한 gameobject
    private GameObject backTile;

    private Material selectedTile;
    private Material summonTile;
    private Material normalTile;
    private Material attackTile;

    private string groundName;
    private string backTileTag;

    private Animator UIpop;

    private Animation anime;

    public int coorR;
    public int coorRU;

    private int[,,] tileCoorInfo;
    private int[,] around6Dir;
    private int[,] path;

    public int whatButton = 0;

    Ray ray;
    RaycastHit rayObj;
    RaycastHit onTile;

    void Awake ()
    {
        selectedTile = system.selected;
        summonTile = system.summonArea;
        normalTile = system.normal;
        attackTile = system.attack;

        attack  = unitsInfo.attack[unitNum];
        hp      = unitsInfo.hp[unitNum];
        speed   = unitsInfo.speed[unitNum];
        control = unitsInfo.control[unitNum];
        range   = unitsInfo.range[unitNum];

        leftSpeed = speed;
        leftHp = hp;

        coorR = 0;
        coorRU = 0;

        if(system.whoseTurn == true)
            came = GameObject.Find("Camera1").GetComponent<Camera>() as Camera;
        else
            came = GameObject.Find("Camera2").GetComponent<Camera>() as Camera;

        unitModel = GetComponent<Transform>().FindChild("modelObj").gameObject;
        unitModel.transform.rotation = Quaternion.Euler(0, 30, 0);

        arrowObj = GetComponent<Transform>().FindChild("Arrow").gameObject;   //오브젝트 하위의 Arrow캔버스를 찾는다 
        unitUIObj = GetComponent<Transform>().FindChild("unitUI").gameObject   //오브젝트 두단계 하위의 UIPanel을 찾는다
            .GetComponent<Transform>().FindChild("UIPanel").gameObject;

        UIpop = GetComponent<Transform>().FindChild("unitUI").gameObject
            .GetComponent<Transform>().FindChild("UIPanel").gameObject.GetComponent<Animator>();//UIPanel에서 Animator컴포넌트를 찾아온다

        anime = unitModel.GetComponent<Animation>();

        movableTile = new GameObject[10][];

        for (int i = 0; i < 10; i++)
        {
            movableTile[i] = new GameObject[(i + 1) * 6];
        }

        tileCoorInfo = new int[10, 60, 2];
        around6Dir = new int[7, 2];
        path = new int[10, 2];
    }

	void Start ()
    {

	}
	

	void Update ()
    {
        if (Physics.Raycast(this.transform.position - Vector3.down * 0.2f, Vector3.down, out onTile, 2)
            && onTile.transform.tag != "OnUnit1" || onTile.transform.tag != "OnUnit2")
        {
            if (tag)
                backTile.tag = backTileTag;
            backTileTag = onTile.transform.tag;
            backTile = onTile.transform.gameObject;
            backTile.tag = "On" + unitModel.tag;
            tag = true;
        }


        if(ctrlSelected == true)                           //컨트롤 권한을 획득
        {
            arrowObj.SetActive(true);                      //화살표 나타나게하는거

            if (unitUIOn == true 
                && Player1.selectCtrl == false)
            {
                unitUIObj.SetActive(true);
                UIpop.SetBool("click", true);
            }
        }
        else if(ctrlSelected == false)                    //컨트롤 권한을 해제
        {
            arrowObj.SetActive(false);                    //화살표 사라지게함
        }

        
        if (whatButton == 1)
        {
            if (unitUIOn == true)
            {
                UIpop.SetBool("click", false);
            }

            if (UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"))
            {
                unitUIOn = false;
                unitUIObj.SetActive(false);
            }

            AttackRange();
        }

        if(whatButton == 2)
        {
            int finishPointR = 0;
            int finishPointRU = 0;



            if (unitUIOn == true)
            {
                UIpop.SetBool("click", false);
            }



            if (UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"))
            {
                unitUIOn = false;
                unitUIObj.SetActive(false);
            }



            if(materialflg == true)
            {
                findMovable(leftSpeed, coorR, coorRU);

                for (int mvrange = 1; mvrange <= leftSpeed; mvrange++)
                {

                    int notExist = 0;

                    for (int tilenum = 1; tilenum <= tileCoorInfo[mvrange, 0, 0]; tilenum++)
                    {

                        if (GameObject.Find("ground(" + tileCoorInfo[mvrange, tilenum, 0] + "," + tileCoorInfo[mvrange, tilenum, 1] + ")") == null)
                        {
                            notExist++;
                        }
                        else if (GameObject.Find("ground(" + tileCoorInfo[mvrange, tilenum, 0] + "," + tileCoorInfo[mvrange, tilenum, 1] + ")") != null)
                        {
                            movableTile[mvrange - 1][tilenum - 1 - notExist] =
                            GameObject.Find("ground(" + tileCoorInfo[mvrange, tilenum, 0] + "," + tileCoorInfo[mvrange, tilenum, 1] + ")");
                        }
                    }
                }

                for (int dist = 0; dist < leftSpeed; dist++)
                {
                    for (int tile = 0; tile < dist * 6 + 6; tile++)
                    {
                        if (movableTile[dist][tile] == null)
                            break;
                        movableTile[dist][tile].GetComponent<MeshRenderer>().material = summonTile;
                    }
                }

                materialflg = false;
            }

            ray = came.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out rayObj, 50)
                && rayObj.transform.tag != "Unit1")
            {
                if(rayObj.transform.GetComponent<MeshRenderer>().material.color == summonTile.color)
                {
                    if (enterFlg == true)
                        back.GetComponent<MeshRenderer>().material = summonTile;  //기존에 ray에 닿았던 타일의 material을 복구
                    back = rayObj.transform.gameObject;                           //ray에 닿은 오브젝트를 back에 할당
                    back.GetComponent<MeshRenderer>().material = selectedTile;    //ray에 닿은 오브젝트의 material을 변경
                    enterFlg = true;
                }
                
            }

            if (Input.GetMouseButton(0))
            {
                enterFlg = false;
 
                groundName = back.name;

                finishPointR = GetR(groundName);
                finishPointRU = GetRU(groundName);

                SearchPath(finishPointR, finishPointRU);
                Move();

                coorR = finishPointR;
                coorRU = finishPointRU;

                system.notMovable[unitCountNum, 0] = coorR;
                system.notMovable[unitCountNum, 1] = coorRU;

                for (int dist = 0; dist < leftSpeed; dist++)
                {
                    for (int tile = 0; tile < dist * 6 + 6; tile++)
                    {
                        if (movableTile[dist][tile] == null)
                            break;
                        movableTile[dist][tile].GetComponent<MeshRenderer>().material = normalTile;
                    }
                }

                for (int dist = 0; dist < leftSpeed; dist++)
                {
                    for (int tile = 0; tile < dist * 6 + 6; tile++)
                    {
                        if (movableTile[dist][tile] == null)
                            break;
                        movableTile[dist][tile] = null;
                    }
                }

                leftSpeed -= path[0, 0];
                whatButton = 0;
                materialflg = true;
                back = null;
            }
        }




        if (whatButton == 5)
        {
            UIpop.SetBool("click", false);
            if(UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"))
            {
                unitUIObj.SetActive(false);
                unitUIOn = false;
                whatButton = 0;
            }
        }

	
	}


    private void findMovable(int leftspd, int R, int RU)
    {
        tileCoorInfo[0, 0, 0] = 1;
        tileCoorInfo[0, 1, 0] = R;  tileCoorInfo[0, 1, 1] = RU;
        
        for (int i = 1; i <= leftspd; i++)
        {
            tileCoorInfo[i, 0, 0] = 0;
        }
        


        for (int dist = 1; dist <= leftspd; dist++)
        {
            for(int t = 1; t <= tileCoorInfo[dist - 1, 0, 0]; t++)
            {
                Around(dist, tileCoorInfo[dist - 1, t, 0], tileCoorInfo[dist - 1, t, 1]);
            }
        }
    }

    private void Around(int layer, int r, int ru)
    {
        around6Dir[0, 0] = r + 1; around6Dir[0, 1] = ru;
        around6Dir[1, 0] = r;     around6Dir[1, 1] = ru + 1;
        around6Dir[2, 0] = r - 1; around6Dir[2, 1] = ru + 1;
        around6Dir[3, 0] = r - 1; around6Dir[3, 1] = ru;
        around6Dir[4, 0] = r;     around6Dir[4, 1] = ru - 1;
        around6Dir[5, 0] = r + 1; around6Dir[5, 1] = ru - 1;

        bool find = false;


        for (int i = 0; i <= 5; i++)
        {
            for (int j = 1; j <= system.unitCount; j++)
            {
                if (around6Dir[i, 0] == system.notMovable[j - 1, 0]
                    && around6Dir[i, 1] == system.notMovable[j - 1, 1])
                {
                    around6Dir[i, 0] = 0; around6Dir[i, 1] = 0;
                    find = true;
                    break;
                }
            }

            if (find == false)
            {
                for (int m = 1; m <= tileCoorInfo[layer, 0, 0]; m++)
                {
                    if (around6Dir[i, 0] == tileCoorInfo[layer, m, 0]
                        && around6Dir[i, 1] == tileCoorInfo[layer, m, 1])
                    {
                        around6Dir[i, 0] = 0; around6Dir[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }

            if(find == false)
            {
                for (int k = 1; k <= tileCoorInfo[layer - 1, 0, 0]; k++)
                {
                    if (around6Dir[i, 0] == tileCoorInfo[layer - 1, k, 0]
                        && around6Dir[i, 1] == tileCoorInfo[layer - 1, k, 1])
                    {
                        around6Dir[i, 0] = 0; around6Dir[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }

            if(find == false && layer >= 2)
            {
                for (int l = 1; l <= tileCoorInfo[layer - 2, 0, 0]; l++)
                {
                    if (around6Dir[i, 0] == tileCoorInfo[layer - 2, l, 0]
                        && around6Dir[i, 1] == tileCoorInfo[layer - 2, l, 1])
                    {
                        around6Dir[i, 0] = 0; around6Dir[i, 1] = 0;
                        find = true;
                        break;
                    }
                }
            }
            find = false;
        }

        int empty = 0;

        for (int n = 0; n <= 5; n++)
        {
            if (around6Dir[n, 0] != 0 || around6Dir[n, 1] != 0)
            {
                tileCoorInfo[layer, tileCoorInfo[layer, 0, 0] + n + 1 - empty, 0] = around6Dir[n, 0];
                tileCoorInfo[layer, tileCoorInfo[layer, 0, 0] + n + 1 - empty, 1] = around6Dir[n, 1];

            }
            else if (around6Dir[n, 0] == 0 && around6Dir[n, 1] == 0)
            {
                empty++;
            }
        }

        tileCoorInfo[layer, 0, 0] += 6 - empty;

    }

    private void SearchPath(int R, int RU)
    {
        int Distance = 0;

        for(int dist = 1; dist <= leftSpeed; dist++)
        {
            for(int i = 1; i <= tileCoorInfo[dist,0,0]; i++)
            {
                if (tileCoorInfo[dist, i, 0] == R && tileCoorInfo[dist, i, 1] == RU)
                {
                    Distance = dist;
                }
            }
        }

        path[0, 0] = Distance;
        path[Distance, 0] = R; path[Distance, 1] = RU; 

        int[,] around = new int[6, 2];

        for (int j = 1; j <= Distance - 1; j++)
        {

            around[0, 0] = path[Distance - j + 1, 0] + 1; around[0, 1] = path[Distance - j + 1, 1];
            around[1, 0] = path[Distance - j + 1, 0];     around[1, 1] = path[Distance - j + 1, 1] + 1;
            around[2, 0] = path[Distance - j + 1, 0] - 1; around[2, 1] = path[Distance - j + 1, 1] + 1;
            around[3, 0] = path[Distance - j + 1, 0] - 1; around[3, 1] = path[Distance - j + 1, 1];
            around[4, 0] = path[Distance - j + 1, 0];     around[4, 1] = path[Distance - j + 1, 1] - 1;
            around[5, 0] = path[Distance - j + 1, 0] + 1; around[5, 1] = path[Distance - j + 1, 1] - 1;

            for (int m = 0; m <= 5; m++)
            {
                for (int n = 1; n <= tileCoorInfo[Distance - j, 0, 0]; n++)
                {
                    if (around[m, 0] == tileCoorInfo[Distance - j, n, 0] && around[m, 1] == tileCoorInfo[Distance - j, n, 1])
                    {
                        path[Distance - j, 0] = around[m, 0]; path[Distance - j, 1] = around[m, 1];
                    }
                }
            }
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

    private void Move()
    {
        for (int move = 1; move <= path[0, 0]; move++)
        {

            Vector3 vectorToNext;

            Quaternion rotationtoNext;

            if(move == 1)
            {
                vectorToNext = Vector3.right * ((path[move, 0] - coorR) * 1.732f
                                                    + (path[move, 1] - coorRU) * 0.866f)
                                    + Vector3.forward * (path[move, 1] - coorRU) * 1.5f;
            }
            else
            {
                vectorToNext = Vector3.right * ((path[move, 0] - path[move - 1, 0]) * 1.732f
                                                    + (path[move, 1] - path[move - 1, 1]) * 0.866f)
                                    + Vector3.forward * (path[move, 1] - path[move - 1, 1]) * 1.5f;
            }

            rotationtoNext = Quaternion.LookRotation(vectorToNext);


            iTween.RotateTo(unitModel, iTween.Hash("y", rotationtoNext.eulerAngles.y, 
                                                    "time", 0.2f,
                                                    "delay", -0.7f + 0.7f * move,
                                                    "easetype", iTween.EaseType.linear));

            iTween.MoveTo(this.gameObject, iTween.Hash("x", path[move, 0] * 1.732f + path[move, 1] * 0.866f,
                                                        "z", path[move, 1] * 1.5f,
                                                        "time", 0.5f,
                                                        "delay", -0.5f + 0.7f * move,
                                                        "easetype", iTween.EaseType.linear,
                                                        "onstart", "MovePlay",
                                                        "oncomplete", "MoveStop")
                                                        );
        }

    }

    private void AttackRange()
    {

        if(materialflg == true)
        {

            for (int i = 1; i <= range; i++)
            {
                
                for (int j = 1; j <= i; j++)
                {

                    tileCoorInfo[i - 1, j * 6 - 6, 0] = coorR + i - j; tileCoorInfo[i - 1, j * 6 - 6, 1] = coorRU + j;
                    tileCoorInfo[i - 1, j * 6 - 5, 0] = coorR + i;     tileCoorInfo[i - 1, j * 6 - 5, 1] = coorRU - i + j;
                    tileCoorInfo[i - 1, j * 6 - 4, 0] = coorR + j;     tileCoorInfo[i - 1, j * 6 - 4, 1] = coorRU - i;
                    tileCoorInfo[i - 1, j * 6 - 3, 0] = coorR - i + j; tileCoorInfo[i - 1, j * 6 - 3, 1] = coorRU - j;
                    tileCoorInfo[i - 1, j * 6 - 2, 0] = coorR - i;     tileCoorInfo[i - 1, j * 6 - 2, 1] = coorRU + i - j;
                    tileCoorInfo[i - 1, j * 6 - 1, 0] = coorR - j;     tileCoorInfo[i - 1, j * 6 - 1, 1] = coorRU + i;
                }
            }

            for(int k = 0; k < range; k++)
            {
                int notExist = 0;

                for(int l = 0; l < k * 6 + 6; l++)
                {
                    if(GameObject.Find("ground(" + tileCoorInfo[k, l, 0] + "," + tileCoorInfo[k, l, 1] + ")") == null)
                    {
                        notExist++;
                    }
                    else if(GameObject.Find("ground(" + tileCoorInfo[k, l, 0] + "," + tileCoorInfo[k, l, 1] + ")") != null)
                    {
                        movableTile[k][l - notExist] = GameObject.Find("ground(" + tileCoorInfo[k, l, 0] + "," + tileCoorInfo[k, l, 1] + ")");
                    }
                }
            }

            for (int dist = 0; dist < range; dist++)
            {
                for (int tile = 0; tile < dist * 6 + 6; tile++)
                {
                    if (movableTile[dist][tile] == null)
                        break;
                    movableTile[dist][tile].GetComponent<MeshRenderer>().material = summonTile;
                }
            }
            materialflg = false;
        }
        
        Ray ray = came.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayObj, 50)
                && rayObj.transform.tag != "Unit1")
        {
            if (rayObj.transform.GetComponent<MeshRenderer>().material.color == summonTile.color)
            {
                if (enterFlg == true)
                    back.GetComponent<MeshRenderer>().material = summonTile;  //기존에 ray에 닿았던 타일의 material을 복구
                back = rayObj.transform.gameObject;                           //ray에 닿은 오브젝트를 back에 할당
                back.GetComponent<MeshRenderer>().material = attackTile;    //ray에 닿은 오브젝트의 material을 변경
                enterFlg = true;
            }

        }
    }

    private void MovePlay()
    {
        anime.Play("RunFront");
    }

    private void MoveStop()
    {
        anime.Play("Idle");
    }
}
