using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{   
    private float rotateMaxTime=5f;
    private float rotateTime=0f;
    private float moveMax=0.4f;
    private float currentMove=0.5f;
    private bool up=true;
    private bool rotateFirstHalf=true;
    private Vector3 poseOne;
    private Vector3 posetwo;

    // Start is called before the first frame update
    void Start()
    {
        poseOne=transform.position;
        posetwo=transform.position;
        poseOne.y +=  moveMax;
        posetwo.y += -moveMax;
    }

    // Update is called once per frame
    void Update()
    {   
        if(rotateTime>=1){
            rotateTime=0f;
            rotateFirstHalf= !rotateFirstHalf;
        }
        if(currentMove>=1){
            currentMove=0f;
            up= !up;
        }
        currentMove += Time.deltaTime/3f;
        rotateTime += Time.deltaTime/rotateMaxTime;
        if(up){
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0),Quaternion.Euler(90, 0, 180), rotateTime);
        }else{
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 180),Quaternion.Euler(90, 0, -0), rotateTime);
        }

        if(up){
            transform.position = Vector3.Lerp(poseOne, posetwo, currentMove);
        }else{
            transform.position = Vector3.Lerp(posetwo, poseOne, currentMove);
        }
    }
}
