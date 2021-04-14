using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{   
    //public Slider sound_slider;
    //public Slider sensitivity_slider;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name=="Options"){//lehetne hogy sring equals ot használok és biztos nem kapok nullt de igy is jó
            Slider sound_slider=GameObject.Find("sound_slider_gui").GetComponent<Slider>();
            if(sound_slider!= null){
                sound_slider.value=Global_options_handler.sound;
            }

            Slider sensitivity_slider=GameObject.Find("sensitivity_slider_gui").GetComponent<Slider>();
            if(sensitivity_slider!= null){
                sensitivity_slider.value=Global_options_handler.sensitivity;
            }
        }
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
        Debug.Log("érzékenység: "+Global_options_handler.sensitivity+" és emez a hang: "+Global_options_handler.sound);
    }
    public void Options(){
        SceneManager.LoadScene(3);//options index
        Debug.Log("érzékenység: "+Global_options_handler.sensitivity+" és emez a hang: "+Global_options_handler.sound);
    }
    public void startStory(){
        SceneManager.LoadScene(2);//story index
    }
    public void quit_from_the_application(){
        Application.Quit();
    }

    public void slider_sound(){
        Slider sound_slider=GameObject.Find("sound_slider_gui").GetComponent<Slider>();
        if(sound_slider!= null){
            Global_options_handler.sound=sound_slider.value;
            AudioListener.volume=Global_options_handler.sound;
        }
    }

    public void slider_sensitivity(){
        Slider sensitivity_slider=GameObject.Find("sensitivity_slider_gui").GetComponent<Slider>();
        if(sensitivity_slider!= null){
            Global_options_handler.sensitivity=sensitivity_slider.value;
            
        }
    }
}
