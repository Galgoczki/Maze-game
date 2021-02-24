using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private bool[,,] maze;
    private GameObject player;
    private float wait;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");

        wait= 100f;
    }

    // Update is called once per frame
    void Update()
    {   
        if(wait>0)
        wait -= Time.deltaTime;
        if(wait<=0){


        }
    }

    int move(){//0 1 2 3
        return 0;
    }

    void gatMaze(bool[,,] basemaze){
        maze=basemaze;
    }
}
