using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch : MonoBehaviour
{
    private Transform player;
    private AudioSource maudio;
    private ParticleSystem ps;
    private bool m_Play=false;
    private ParticleSystem my_particle;
    private Light my_light;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {   
        my_light = GetComponentInChildren<Light>();
        player = GameObject.Find("player").transform;
        maudio=GetComponentInChildren<AudioSource>();
        my_particle = GetComponentInChildren<ParticleSystem>();
        

        maudio.Stop();
        my_particle.Stop();
        m_Play=false;
        my_light.enabled = false;
        my_light.intensity=0;
        distance= Random.Range(15f, 30f);

    }

    // Update is called once per frame
    void Update()
    {
        float tavolsag =Vector3.Distance(player.position, transform.position);
        if(tavolsag<=distance){
            if(Global_options_handler.horroristic){
                if(Global_options_handler.lightoff){
                    my_light.enabled = false;
                }else{
                    if(!my_light.enabled){
                        my_light.enabled=true;
                    }
                }
            }
            if(!m_Play){
                maudio.Play();
                m_Play=true;
                my_particle.Play();
                my_light.enabled = true;
                //Debug.Log("be");
            }
            my_light.intensity = Mathf.Lerp(0, 2.0f,tavolsag/distance);
            //Debug.DrawLine(this.transform.position,  this.transform.position+Vector3.up,Color.green,0.3f);
        }else{
            if(m_Play){
                maudio.Stop();
                my_particle.Stop();
                m_Play=false;
                my_light.enabled = false;
                //Debug.Log("ki");
            }
            //Debug.DrawLine(this.transform.position,  this.transform.position+Vector3.up,Color.red,0.3f);
            
        }
    }
}
