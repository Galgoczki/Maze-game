using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{ 
    private float moveMax=0.2f;
    private float currentMove=0f;
    private bool up=true;
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
        if(currentMove>=1){
            currentMove=0f;
            up= !up;
        }
        
        currentMove += Time.deltaTime/3f;

        if(up){
            transform.position = Vector3.Lerp(poseOne, posetwo, currentMove);
        }else{
            transform.position = Vector3.Lerp(posetwo, poseOne, currentMove);
        }
    }
}
