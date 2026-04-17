using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D playerrb;
    private Vector2 moveInput;

    public Vector2 lastDirection = Vector2.right;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerrb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        if(moveX !=0 || moveY != 0)
        {
            lastDirection = new Vector2(moveX,moveY).normalized;
        }

        
    }
    private void FixedUpdate()
    {
        playerrb.linearVelocity = moveInput * moveSpeed; 
    }

    
}
