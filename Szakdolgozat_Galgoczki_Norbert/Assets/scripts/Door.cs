using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    private bool nyitva=false;
    private float autoclose=0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(autoclose>0)
            autoclose-=Time.deltaTime;
        
        //if(autoclose<0&&nyitva)
        //    csuk();
    }

    public void open_door(){
        if(!nyitva){
            autoclose=3f;
            nyitva=true;
            animator.SetTrigger("nyit");
        }else{
            autoclose=3f;
            nyitva=false;
            animator.SetTrigger("csuk");
        } 
        
    }
    public bool nyithato(){
        if(autoclose<=0.1f){
            return true;
        }else{
            return false;
        }
    }
    private void csuk(){
        autoclose=0f;
        nyitva=false;
        Debug.Log("csukivan");
        animator.SetTrigger("csuk");
    }
}
