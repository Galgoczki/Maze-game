using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch : MonoBehaviour
{
    private Transform player;
    private AudioSource maudio;
    private bool m_Play=false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").transform;
        maudio=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position , this.transform.position)<5){
            if(!m_Play){
                maudio.Play();
                m_Play=true;
            }
            
            //Debug.DrawLine(this.transform.position,  this.transform.position+Vector3.up,Color.green,0.3f);
        }else{
            if(m_Play){
                maudio.Stop();
                m_Play=false;
            }
            //Debug.DrawLine(this.transform.position,  this.transform.position+Vector3.up,Color.red,0.3f);
            
        }
    }
}
