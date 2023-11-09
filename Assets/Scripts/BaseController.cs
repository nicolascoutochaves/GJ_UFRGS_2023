
public class BaseController
{
    
    public int dx, dy;
    public int facingx = 1, facingy;

    public virtual void Move(){
        
    }

    public virtual bool JumpPress(){
        return false;  
    }
    public virtual bool JumpRealese(){
        return false;  
    }
    public virtual bool Aim(){
        return false;
    }
    public virtual bool CastSpell(){
        return false;
    }

  
}
