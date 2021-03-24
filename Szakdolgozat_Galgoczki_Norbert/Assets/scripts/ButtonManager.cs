using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startGame(){
        SceneManager.LoadScene(1);//maze index (build setting fenti lista)
        
    }
    public void Menu(){
        SceneManager.LoadScene(0);//menu index
    }
    public void startStory(){
        SceneManager.LoadScene(2);//story index
    }
}
