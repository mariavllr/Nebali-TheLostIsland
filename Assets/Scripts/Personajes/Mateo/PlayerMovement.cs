using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform cam;
    [SerializeField] Animator animator;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f; //Para que gire de manera suave
    private float turnSmoothVelocity;

    public bool canMove;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //No queremos movernos hacia arriba

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walking", true);
                //Calcular el �ngulo al que est� mirando y para que gire siempre donde apunta la camara
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                //Direcci�n donde tiene que moverse seg�n el �ngulo calculado
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


                controller.Move(moveDir.normalized * speed * Time.deltaTime);

            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }    
    }
}