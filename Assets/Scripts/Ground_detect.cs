using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_detect : MonoBehaviour
{
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
         
        if(other.gameObject.CompareTag("Ground") || other.gameObject.layer == 3)
            isGrounded = true; //Permite que o player pule apenas quando estiver no layer do ground
 
    }
    private void OnCollisionExit2D(Collision2D other) {
         if(other.gameObject.CompareTag("Ground") || other.gameObject.layer == 3)
            isGrounded = false;
         
    }
}
