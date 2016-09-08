using UnityEngine;
using System.Collections;

public class OnControl : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}

    void OnEnable()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(25.098f, 1, 13.5f), 0.5f * Time.deltaTime);
    }
}
