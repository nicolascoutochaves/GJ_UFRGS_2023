using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 15f, JumpForce = 2f, jump_delay = 0.2f;
    [SerializeField] private GameObject shot, aim; //Referencias aos objetos do tiro e da mira
    [SerializeField] private Spell spell; //Referencia ao script Spell
    [SerializeField] private int lives = 3;
    private GameObject arrow; //Objeto temporario para instanciar a mira
    private bool isGrounded, isJumping = false, canJump = true, isShooting;
    private float health, jump_timer;

    private Rigidbody2D rigid;
    private BaseController controller;
    
 
    // Start is called before the first frame update
    void Start()
    {   
        
            rigid = GetComponent<Rigidbody2D>();
        

        #region Pick_Player_Controller
            if(gameObject.name == "Player_1"){
                controller = new C1_Controller();     
            }

            if(gameObject.name == "Player_2"){
                controller = new C2_Controller();
            }
        #endregion


    }

    //Physics Update
    private void FixedUpdate() {
      
        rigid.velocity = new Vector2(speed * controller.dx , rigid.velocity.y); //Atualiza a velocidade do jogador com a direcao recebida do controle

        if(isJumping){
                jump_timer += Time.deltaTime;
                rigid.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
        

        //Store last player facing direction 
        if(controller.dx != 0 || controller.dy != 0){
            controller.facingx = controller.dx;
            controller.facingy = controller.dy;
        }

        //Stop player movement after input command stops
        controller.dx = 0;
        controller.dy = 0;
    }

    // Update is called once per frame
    void Update()
    {

        #region Controller_Realated
            controller.Move();

            if(isGrounded){
                canJump = true;
            }
            if(controller.JumpPress() && isGrounded && canJump){
                isJumping = true;
                jump_timer = 0;
            } 
            
            if(jump_timer > jump_delay || controller.JumpRealese()){
                isJumping = false;
                canJump = false;
            }
          
            #region Shots

                if(controller.Aim()){ //Verifica se o botao de tiro foi pressionado. Ativa o modo de mira
                    arrow = Instantiate(aim, this.transform);
                        isShooting = true; 
                }

                if(isShooting){
                    Aim(controller.facingx, controller.facingy, arrow, gameObject.transform); //chama a funcao que instancia a flecha
                }

                if(controller.CastSpell()){ //Verifica se o botao de tiro foi liberado
                    isShooting = false;
                    Destroy(arrow);
                    if(spell.timer <= 0){
                        Shot(controller.facingx, controller.facingy, gameObject.transform); //Funcao que atira para posicao que o player estava olhando
                    }
                }
                if(spell.timer > 0){
                    spell.timer -= Time.deltaTime; //Deley para atirar novamente
                }
            #endregion
                  
        #endregion
   
        #region LivesAndHealth
            if(health < 0){
                ResetPosition();
                health = 0;
                lives --;
            }
        #endregion

    }

    public void Aim(int dx, int dy, GameObject aim, Transform player){
        float angle = Mathf.Atan2(dy , (dx+0.001f)) * (180/Mathf.PI); //Calculo do angulo da seta
        aim.transform.position = player.position; //Faz a mira seguir o jogador
        aim.transform.eulerAngles = new Vector3(aim.transform.eulerAngles.x, aim.transform.eulerAngles.y, angle-90); //muda o angulo da mira
    }
    public void Shot(int dx, int dy, Transform player){
        GameObject shot_temp = Instantiate(shot, this.transform); //Instancia o tiro como sendo uma child do player usando uma variavel temporaria
        shot_temp.transform.position = player.position + new Vector3(dx, dy, 0);
        shot_temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(rigid.velocity.x + dx * spell.speed, rigid.velocity.y + dy * spell.speed), ForceMode2D.Impulse); //Velocidade do disparo proporcional a do jogador
        spell.timer = spell.cooldown;
        Destroy(shot_temp, spell.aliveTime);
    }

    void ResetPosition(){ //Reset player position
        transform.position = new Vector3(0f, 0f, 0f);
        rigid.velocity = new Vector2(0f, 0f);
    }
   

    //Player Collision Events

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Destroyer"){ //Verifica se o player saiu da regiao do mapa (Usei 4 trigger box nas 4 direcoes do mapa com a tag "Destroyer" -> as trigger box tem que ser bem grandes pq dependendo da velocidade que o player e arremessado, nao da tempo de verificar a colisao)
            health = -1; //-1 porque a vida comeca em zero e vai aumentando (tipo smash bros)
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        //Verifica se o player esta no chao (Obsoleta)
        if(other.gameObject.layer == 3)
            isGrounded = true; //Permite que o player pule apenas quando estiver no layer do ground



        if(other.gameObject.tag == "Spell"){ //Colisao com os objetos da tag tiro

            //Verifica se o tiro nao e o tiro do proprio jogador
            if(!other.gameObject.transform.IsChildOf(this.transform)){
                //Usa as velocidades do tiro para aplicar o knockback
                float velocityx = other.gameObject.GetComponent<Rigidbody2D>().velocity.x;
                float velocityy = other.gameObject.GetComponent<Rigidbody2D>().velocity.y;

                Vector2 knockback = new Vector2(health + velocityx, health +  velocityy); //Adiciona knockback se baseando na vida do player
                rigid.velocity = knockback;
                Destroy(other.gameObject);
                health += spell.damage;
            }
        }
        
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.layer == 3)
            isGrounded = false;
        
    }

}
