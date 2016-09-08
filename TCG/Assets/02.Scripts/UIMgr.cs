using UnityEngine;
using System.Collections;

public class UIMgr : MonoBehaviour
{
    public static int unitUIBtn = 0;
    

    public void OnClickAttack()
    {
        (this.gameObject.transform.root.gameObject.GetComponent<PlayArcher>() as PlayArcher).whatButton = 1;
    }
	
    public void OnClickMove()
    {
        (this.gameObject.transform.root.gameObject.GetComponent<PlayArcher>() as PlayArcher).whatButton = 2;
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
        (this.gameObject.transform.root.gameObject.GetComponent<PlayArcher>() as PlayArcher).whatButton = 5;
        
    }
}
