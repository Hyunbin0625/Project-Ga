using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    public Transform target;    // Player 정보
 
    // Update is called once per frame
    void Update()
    {
        // 데드존의 위치 = 플레어의 위치의 x, z축, 자기자신의 위치의 y축
        transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
       
        // 데드존 회전 = player 회전
        transform.rotation = target.rotation;
    }
}
