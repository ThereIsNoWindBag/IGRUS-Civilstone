using UnityEngine;
using System.Collections;

public class playArcher : MonoBehaviour
{
    public static int unitNum = 0;              //유닛 번호

    public static int attack;
    public static int hp;
    public static int speed;
    public static int control;
    public static int range;

    public static bool ctrlSelected = false;    //컨트롤 권한 여부

    private GameObject arrowObj;                //Arrow 캔버스를 오브젝트로 담을 변수
    private GameObject unitUIObj;               //unitUI 캔버스를 오브젝트로 담을 변수

    public static bool unitUIOn = false;        //unitUI 표시 여부

    private Animator UIpop;


    void Awake ()
    {
        attack  = unitsInfo.attack[unitNum];
        hp      = unitsInfo.hp[unitNum];
        speed   = unitsInfo.speed[unitNum];
        control = unitsInfo.control[unitNum];
        range   = unitsInfo.range[unitNum];

        arrowObj = GetComponent<Transform>().FindChild("Arrow").gameObject;   //오브젝트 하위의 Arrow캔버스를 찾는다
        unitUIObj = GetComponent<Transform>().FindChild("/Archer Girl/unitUI/UIPanel").gameObject; //오브젝트 하위의 UIPanel을 찾는다

        UIpop = GetComponent<Transform>().FindChild("/Archer Girl/unitUI/UIPanel").gameObject.GetComponent<Animator>(); //하위 항목의 UIPanel에서 Animator컴포넌트를 찾아온다

    }

	void Start ()
    {
        
	}
	

	void Update ()
    {

        if(ctrlSelected == true)                           //컨트롤 권한을 획득
        {
            arrowObj.SetActive(true);                      //화살표 나타나게하는거

            if (unitUIOn == true && !Player1.selectCtrl)
            {
                unitUIObj.SetActive(true);
                UIpop.SetBool("click", true);
            }
        }
        else if(ctrlSelected == false)                    //컨트롤 권한을 해제
        {
            arrowObj.SetActive(false);                    //화살표 사라지게함
        }

        
        if (UIMgr.unitUIBtn == 5)
        {
            UIpop.SetBool("click", false);
            Debug.Log(UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"));
            if(UIpop.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.NonSelected"))
            {
                UIMgr.unitUIBtn = 0;
                unitUIObj.SetActive(false);
                unitUIOn = false;
                Debug.Log("1");
            }
        }

	
	}

}
