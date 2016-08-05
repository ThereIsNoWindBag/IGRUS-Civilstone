using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour
{
    public static int unitUIBtn = 0;

    public void OnClickAttack()
    {
        unitUIBtn = 1;
    }
	
    public void OnClickMove()
    {
        unitUIBtn = 2;
    }

    public void OnClickSkill()
    {
        unitUIBtn = 3;
    }

    public void OnClickInfo()
    {
        unitUIBtn = 4;
    }

    public void OnclickCancel()
    {
        unitUIBtn = 5;
    }
}
