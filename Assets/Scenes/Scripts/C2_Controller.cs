using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2_Controller : BaseController
{
    public override void Move(){
        if(Input.GetKey(KeyCode.RightArrow))
            dx = 1;
        if(Input.GetKey(KeyCode.LeftArrow))dx = -1;
        if(Input.GetKey(KeyCode.UpArrow))
            dy = 1;
        if(Input.GetKey(KeyCode.DownArrow))
            dy = -1;

    }
    public override bool JumpPress(){
        return (Input.GetKeyDown(KeyCode.Mouse1));   
    }
    public override bool JumpRealese(){
        return (Input.GetKeyUp(KeyCode.Mouse1));   
    }

    public override bool Aim(){
        return (Input.GetKeyDown(KeyCode.Mouse0));
    }
    public override bool CastSpell(){
        return (Input.GetKeyUp(KeyCode.Mouse0));
    }
}
