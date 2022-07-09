using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groudCheckTransform;
    [SerializeField] private LayerMask playerMask;
    private bool shouldJump;
    private float verticalInput;
    private float horizontalInput;
    private Rigidbody rigidbodyComponent;
    private int superJumpsRemaining;
    private float walkSpeed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            shouldJump = true;
        }
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // Fixed update called once every physics update
    private void FixedUpdate() {
        rigidbodyComponent.velocity = new Vector3(horizontalInput*walkSpeed, rigidbodyComponent.velocity.y, verticalInput*walkSpeed);

        if (Physics.OverlapSphere(groudCheckTransform.position, 0.1f, playerMask).Length == 0){
            return;
        }

        if (shouldJump) {
            float jumpPower = 6f;
            if (superJumpsRemaining > 0){
                jumpPower *= 1.5f;
                superJumpsRemaining--;
            }
            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            shouldJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) {
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }
}
