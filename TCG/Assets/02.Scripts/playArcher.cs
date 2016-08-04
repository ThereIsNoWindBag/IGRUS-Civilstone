using UnityEngine;
using System.Collections;

public class playArcher : MonoBehaviour
{
    int unitNum = 1; //유닛 번호

    void Awake ()
    {
        int attack  = unitsInfo.attack[unitNum];
        int hp      = unitsInfo.hp[unitNum];
        int speed   = unitsInfo.speed[unitNum];
        int control = unitsInfo.control[unitNum];
        int range   = unitsInfo.range[unitNum];
    }
	void Start ()
    {
        
	}
	

	void Update ()
    {
	
	}
}
