using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Global_options_handler{
    private float sound;
    private float sensitivity;

    public void setSound(float sound){
        this.sound=sound;
    }

    public void setSensitivity(float sensitivity){
        this.sensitivity=sensitivity;
    }
    public float getSound(){
        return this.sound;
    }

    public float getSensitivity(){
        return this.sensitivity;
    }

}
