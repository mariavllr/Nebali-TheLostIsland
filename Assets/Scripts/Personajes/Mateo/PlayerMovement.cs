using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform cam;
    [SerializeField] Animator animator;

    public float normalSpeed = 15f;
    public float runningSpeed = 40f;
    public float turnSmoothTime = 0.1f; //Para que gire de manera suave
    public float speed;
    private float turnSmoothVelocity;
    private float gravity = -9.8f;
    private float verticalVelocity;
    private AudioSource audio;

    public bool canMove;
    public bool isMoving;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        canMove = true;
        isMoving = false;
        speed = normalSpeed;
    }

    void Update()
    {
        verticalVelocity = gravity * Time.deltaTime;


        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //No queremos movernos hacia arriba

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walking", true);

                //Si pulsa shift, correr
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetBool("Running", true);
                    speed = runningSpeed;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    animator.SetBool("Running", false);
                    speed = normalSpeed;
                }
                
                isMoving = true;

                //Calcular el ángulo al que está mirando y para que gire siempre donde apunta la camara
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                //Dirección donde tiene que moverse según el ángulo calculado
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                moveDir.y = verticalVelocity;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                if (!audio.isPlaying) audio.Play();

            }
            else
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Running", false);
                isMoving = false;
                audio.Stop();
                controller.Move(new Vector3(0, verticalVelocity, 0));
            }
        }    
    }

}
