using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Dog dog1;
    private Dog dog2;
    private int my_position_i;
    private int my_position_j;

    public void registrat(Dog firstDog,Dog secundDog,int i,int j){
        dog1=firstDog;
        dog2=secundDog;
        my_position_i=i;
        my_position_j=j;
    }
    public void playerEnter(){
        dog1.setPlayerPosition(my_position_i,my_position_j);
        dog2.setPlayerPosition(my_position_i,my_position_j);
    }
    public void playerExit(){
        dog1.setPlayerPosition(-1,-1);
        dog2.setPlayerPosition(-1,-1);
    }
}
