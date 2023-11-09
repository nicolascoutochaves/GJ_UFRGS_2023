
public class BaseController
{
    
    public int dx, dy;
    public int facingx = 1, facingy;

    public virtual void Move(){ //Botoes de movemento
        
    }

    public virtual bool JumpPress(){ //Verifica se o botao de pulo foi pressionado
        return false;  
    }
    public virtual bool JumpRealese(){ //Verifica se  o botao de pulo foi solto (A altura e dada pelo tempo em que o botao ficou pressionado)
        return false;  
    }
    public virtual bool Aim(){ //Verifica se o botao de tiro foi pressionado (ativa modo de mira)
        return false;
    }
    public virtual bool CastSpell(){ //Verifica se o botao de tiro foi solto (Desativa a mira e dispara o projetil)
        return false;
    }

  
}
