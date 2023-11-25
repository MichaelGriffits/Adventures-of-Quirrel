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
    //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
    public PlayerMovement character;


    //Run this method instead of Initialization if you don't have any scripts inheriting from each other
    void Start()
    {
        //The Animator component on the player
        PlayerANIM = GetComponent<Animator>();
        //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
        PlayerMovement character = new PlayerMovement();
        //The animator on the meleePrefab
        meleeAnimator = GameObject.FindGameObjectWithTag("weapon").GetComponent<Animator>();
        
    }   


    // protected override void Initialization()
    // {
    //     //This grabs all the references already deinde by the PlayerMovement script
    //     base.Initialization();
    //     //The animator on the meleePrefab
    //     meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
    // }

    void Update()
    {
        //Method that checks to see what keys are being pressed
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

    void CheckInput()
    {
        //Checks to see if Backspace key is pressed which I define as melee attack; you can of course change this to anything you would want
        if (Input.GetKeyDown(KeyCode.J))
        {
            //Sets the meleeAttack bool to true
            meleeAttack = true;
            if (meleeAttack && Input.GetAxis("Vertical") > 0)
            {
                //Turns on the animation for the player to perform an upward melee attack
                PlayerANIM.SetTrigger("UpwardMelee");
                //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
                meleeAnimator.SetTrigger("UpwardMeleeSwipe");
            }
            //Checks to see if meleeAttack is true and pressing down while also not grounded
            if (meleeAttack && Input.GetAxis("Vertical") < 0 && !character.isGrounded)
            {
                //Turns on the animation for the player to perform a downward melee attack
                PlayerANIM.SetTrigger("DownwardMelee");
                //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
                meleeAnimator.SetTrigger("DownwardMeleeSwipe");
            }
            //Checks to see if meleeAttack is true and not pressing any direction
            if ((meleeAttack && Input.GetAxis("Vertical") == 0)
                 //OR if melee attack is true and pressing down while grounded
                || meleeAttack && (Input.GetAxis("Vertical") < 0 && character.isGrounded))
            {
                //Turns on the animation for the player to perform a forward melee attack
                PlayerANIM.SetTrigger("ForwardMelee");
                //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
                meleeAnimator.SetTrigger("ForwardMeleeSwipe");
            }
        }
        else
        {
            //Turns off the meleeAttack bool
            meleeAttack = false;
        }
        //Checks to see if meleeAttack is true and pressing up
        
    }
}   
