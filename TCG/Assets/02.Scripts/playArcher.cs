using UnityEngine;
using System.Collections;

public class playArcher : MonoBehaviour
{
    int unitNum = 1; //유닛 번호

    void Awake ()
    {
        int attack  = UnitInfo.attack[unitNum];
        int hp      = UnitInfo.hp[unitNum];
        int speed   = UnitInfo.speed[unitNum];
        int control = UnitInfo.control[unitNum];
        int range   = UnitInfo.range[unitNum];
    }
	void Start ()
    {
        
	}
	

	void Update ()
    {
	
	}
}
