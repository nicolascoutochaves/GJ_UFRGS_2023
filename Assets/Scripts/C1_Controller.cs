using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1_Controller : BaseController
{
    public override void Move(){
        if(Input.GetKey(KeyCode.D))
            dx = 1;
        if(Input.GetKey(KeyCode.A))
            dx = -1;
        if(Input.GetKey(KeyCode.W))
            dy = 1;
        if(Input.GetKey(KeyCode.S))
            dy = -1;

    }
    public override bool JumpPress(){
        return (Input.GetKeyDown(KeyCode.H));   
    }
    public override bool JumpRealese(){
        return (Input.GetKeyUp(KeyCode.H));   
    }

    public override bool Aim(){
        return (Input.GetKeyDown(KeyCode.J));
    }

    public override bool CastSpell(){
        return (Input.GetKeyUp(KeyCode.J));
    }

  
}
