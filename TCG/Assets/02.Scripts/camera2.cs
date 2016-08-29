using UnityEngine;
using System.Collections;

public class camera2 : MonoBehaviour {

    private Transform came;
    public float wheelSpeed = 500f; //휠 속도
    public float screenSpeed = 15.0f; //스크린 속도

    private Vector3 mousePosition; //마우스 좌표값 받을 변수
    private float scrLside;
    private float scrRside;
    private float scrUside;
    private float scrDside;

    private Vector3 screenUp; //스크린 위쪽방향

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;// 마우스가 스크린 밖으로 이동 못하게 함 , 에디터에서는 적용되지 않는다고 함...

        came = GetComponent<Transform>();

        //mousePosition = Vector3.zero; //이거 필요있음?
        scrLside = Screen.width / 200;
        scrRside = Screen.width * 199 / 200;
        scrUside = Screen.height * 199 / 200;
        scrDside = Screen.height / 200;

        screenUp = (Vector3.up * 2 + Vector3.forward).normalized;   //크기 1로 정규화
    }

    void Update()
    {
        if (came.position.y <= 15 && Input.GetAxis("Mouse ScrollWheel") < 0) //줌을 올릴때 동작함 , 최대줌 15
        {
            came.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime);// 카메라 줌 조절
        }
        if (came.position.y >= 5 && Input.GetAxis("Mouse ScrollWheel") > 0) //줌을 당길때 동작함 , 최소줌 5
        {
            came.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime);
        }

        mousePosition = Input.mousePosition; //마우스 좌표값

        //카메라 전후좌우 이동
        if (mousePosition.y >= scrDside && mousePosition.y <= scrUside)
        {
            if (mousePosition.x <= scrLside)
            {
                came.Translate(Vector3.left * screenSpeed * (came.position.y / 10) * Time.deltaTime); //왼쪽 이동
            }
            else if (mousePosition.x >= scrRside)
            {
                came.Translate(Vector3.right * screenSpeed * (came.position.y / 10) * Time.deltaTime); //오른쪽 이동
            }
        }

        if (mousePosition.x >= scrLside && mousePosition.x <= scrRside)
        {
            if (mousePosition.y <= scrDside)
            {
                came.Translate(-screenUp * screenSpeed * (came.position.y / 10) * Time.deltaTime); //아래로 이동
            }

            else if (mousePosition.y >= scrUside)
            {
                came.Translate(screenUp * screenSpeed * (came.position.y / 10) * Time.deltaTime); //위로 이동
            }
        }

        //카메라 대각선 이동 , 벡터 크기 정규화
        if (mousePosition.x <= scrLside && mousePosition.y <= scrDside)
        {
            came.Translate((Vector3.left - screenUp).normalized * screenSpeed * (came.position.y / 10) * Time.deltaTime); //아래 왼쪽
        }

        else if (mousePosition.x <= scrLside && mousePosition.y >= scrUside)
        {
            came.Translate((Vector3.left + screenUp).normalized * screenSpeed * (came.position.y / 10) * Time.deltaTime); //위 왼쪽
        }

        else if (mousePosition.x >= scrRside && mousePosition.y <= scrDside)
        {
            came.Translate((Vector3.right - screenUp).normalized * screenSpeed * (came.position.y / 10) * Time.deltaTime); //아래 오른쪽
        }

        else if (mousePosition.x >= scrRside && mousePosition.y >= scrUside)
        {
            came.Translate((Vector3.right + screenUp).normalized * screenSpeed * (came.position.y / 10) * Time.deltaTime); //왼쪽 위
        }
    }
}