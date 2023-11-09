using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 15f, JumpForce = 2f, jump_delay = 0.2f;
    [SerializeField] private GameObject spell, aim;
    private GameObject arrow; //temporary object for aim
    private bool isGrounded, isJumping = false, canJump = true, isShooting;
    private float health = 100f, jump_timer = 0;

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
      
        rigid.velocity = new Vector2(speed * controller.dx , rigid.velocity.y);

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
          
            
            if(controller.Aim()){
                arrow = Instantiate(aim);
                isShooting = true; 
            }
            if(isShooting){
                Aim(controller.facingx, controller.facingy, arrow, gameObject.transform);
            }

            if(controller.CastSpell()){
                isShooting = false;
                Shot(controller.facingx, controller.facingy, gameObject.transform); 
            }
                  
            if(Input.GetKeyDown(KeyCode.R))
                ResetPosition();
        #endregion
   
        #region LivesAndHealth
            if(health <= 0){
                ResetPosition();
                health = 100;
            }
        #endregion

    }

    public void Aim(int dx, int dy, GameObject aim, Transform player){
        float angle = Mathf.Atan2(dy , (dx+0.001f)) * (180/Mathf.PI);
        aim.transform.position = player.position;
        //aim.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.51f);
        //aim.GetComponent<RectTransform>().eulerAngles = new Vector3(aim.transform.eulerAngles.x, aim.transform.eulerAngles.y, angle-90);
        aim.transform.eulerAngles = new Vector3(aim.transform.eulerAngles.x, aim.transform.eulerAngles.y, angle-90);
        Debug.Log(angle);
    }
    public void Shot(int dx, int dy, Transform player){
        Destroy(arrow);
        GameObject temp = Instantiate(spell);
        temp.transform.position = player.position + new Vector3(dx, dy, 0);
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(rigid.velocity.x + dx * speed, rigid.velocity.y + dy * speed), ForceMode2D.Impulse);
        
        Destroy(temp, 3);
    }

    void ResetPosition(){ //Reset player position (just for tests)
        transform.position = new Vector3(0f, 0f, 0f);
        rigid.velocity = new Vector2(0f, 0f);
    }
  

    //Player Collision Events

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == 6){ //Verifica se o layer e do destroyer para matar o jogador
            health = 0;
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3)
            isGrounded = true; //Permite que o player pule apenas quando estiver no layer do ground
        
        
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.layer == 3)
            isGrounded = false;
        
    }

}
