using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour{
    private Animator animator;
    private bool nyitott = false;
    // Start is called before the first frame update
    void Start(){
        animator = this.GetComponent<Animator>();
        animator.SetBool("open", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isOpen(){
        return nyitott;
    }
    public void open_chest(){
        nyitott = true;
        animator.SetBool("open", true);
    }
}
