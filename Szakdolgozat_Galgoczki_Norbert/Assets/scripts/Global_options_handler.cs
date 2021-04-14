using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Global_options_handler{
    private float sound;
    float sensitivity;

    void setSound(float sound){
        this.sound=sound;
    }

    void setSensitivity(float sensitivity){
        this.sensitivity=sensitivity;
    }
    float getSound(){
        return this.sound;
    }

    float getSensitivity(){
        return this.sensitivity;
    }

}
