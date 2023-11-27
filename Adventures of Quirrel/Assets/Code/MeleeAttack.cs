using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : PlayerMovement
{
    //How much the player should move either downwards or horizontally when melee attack collides with a GameObject that has EnemyHealth script on it
    public float defaultForce = 300;
    //How much the player should move upwards when melee attack collides with a GameObject that has EnemyHealth script on it
    public float upwardsForce = 600;
    //How long the player should move when melee attack collides with a GameObject that has EnemyHealth script on it
    public float movementTime = .1f;
    //Input detection to see if the button to perform a melee attack has been pressed
    private bool meleeAttack;
    //The animator on the meleePrefab
    public Animator meleeAnimator;
    //The Animator component on the player
    public Animator PlayerANIM;
    //The Character script on the player
    public PlayerMovement character;

    //Function that runs on start of code
    //Sets the animator componenet up
    //Sets the varilbe Character up so it can access the playermovement script
    //Sets th melee animator component up
    void Start()
    {
        PlayerANIM = GetComponent<Animator>();
        PlayerMovement character = new PlayerMovement();
        meleeAnimator = GameObject.FindGameObjectWithTag("weapon").GetComponent<Animator>();
    }   

    //Function that runs every frame
    //Runs the CheckInput function aswell as the AnimatorController function
    //Checks if the player is moving
    void Update()
    {
        CheckInput();
        AnimatorController();   
     	
        if(GetComponent<Rigidbody2D>().velocity.magnitude < 0.5)
		{
			IsRunning = false;
			IsIdle = true;
		} 
		else if(GetComponent<Rigidbody2D>().velocity.magnitude > 0.5)
        {
			IsRunning = true;
			IsIdle = false;
		}
		
    }

    //Function that runs in the update
    //Checks what kind of directional input is be inputed by the user aswell as if it is grounded
    //Depending on which it is it plays different animations
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            meleeAttack = true;
            if (meleeAttack && Input.GetAxis("Vertical") > 0)
            {
                PlayerANIM.SetTrigger("UpwardMelee");
                meleeAnimator.SetTrigger("UpwardMeleeSwipe");
            }
            if (meleeAttack && Input.GetAxis("Vertical") < 0 && !character.isGrounded)
            {
                PlayerANIM.SetTrigger("DownwardMelee");
                meleeAnimator.SetTrigger("DownwardMeleeSwipe");
            }
            if ((meleeAttack && Input.GetAxis("Vertical") == 0) || meleeAttack && (Input.GetAxis("Vertical") < 0 && character.isGrounded))
            {
                PlayerANIM.SetTrigger("ForwardMelee");
                meleeAnimator.SetTrigger("ForwardMeleeSwipe");
            }
        }
        else
        {
            meleeAttack = false;
        }        
    }
}   
