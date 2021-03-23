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
        autoclose-=Time.deltaTime;
        if(autoclose<0&&nyitva)
            csuk();
    }

    public void open_door(){
        autoclose=10f;
        nyitva=true;
        Debug.Log("nyilivan");
        animator.SetTrigger("nyit");
        
    }

    private void csuk(){
        autoclose=0f;
        nyitva=false;
        Debug.Log("csukivan");
        animator.SetTrigger("csuk");
    }
}
