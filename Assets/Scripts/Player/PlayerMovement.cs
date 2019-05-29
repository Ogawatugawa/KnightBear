﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float baseSpeed = 5f;
    public float moveSpeed;
    public static Vector2 motion;
    public static Vector2 direction = new Vector2(0, -1);
    public static bool CanMove = true;

    [Header("Dodge Variables")]
    public float dodgeSpeed;
    public float maxDodgeTime, preDodgeDelay, afterDodgeDelay;
    private float dodgeTimer;
    public static bool isDodging;
    private Vector2 dashDirection;

    private Animator anim;
    private CharacterController2D charC;
    private SpriteRenderer rend;

    void Start()
    {
        anim = GetComponent<Animator>();
        charC = GetComponent<CharacterController2D>();
        rend = GetComponent<SpriteRenderer>();

        dodgeTimer = maxDodgeTime;
    }

    void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        if (CanMove)
        {
            Move(inputH, inputV);

            FaceDirection(inputH, inputV);

            // Run Dodge() which will dodge if Left Shift is pressed
            Dodge();

            //Move using Character Controller function
            charC.Move(motion * Time.deltaTime);
        }
    }

    void Move(float inputH, float inputV)
    {
        // Multiply Motion by Move Speed
        motion.x = inputH * moveSpeed;
        motion.y = inputV * moveSpeed;
        // Set our animator float Motion
        anim.SetFloat("Motion", motion.magnitude);
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            // Store the direction we were facing when the dodge started
            dashDirection = direction;
            // Delay movement by the pre-dodge penalty
            StartCoroutine(PreDodgeDelay(preDodgeDelay));
            // Set our dash timer to 0
            dodgeTimer = 0;
        }

        // If Dash is active, i.e. our dash timer is on and counting up
        if (dodgeTimer < maxDodgeTime)
        {
            direction = dashDirection;
            // Motion becomes our last faced direction multiplied by our dash speed
            motion = direction * dodgeSpeed;
            // Count up the dash timer by Time.deltaTime
            dodgeTimer += Time.deltaTime;
            //Set IsDodging to true
            isDodging = true;
            // Keep the cooldown timer to 0 so Dash doesn't start cooling down until the Dash is completed
            //cooldownTimer = 0;
        }

        // Else if our dodge time is up and we're still flagged as dodging
        else if (dodgeTimer > maxDodgeTime && isDodging)
        {
            // Stop moving
            motion = Vector2.zero;
            // Delay movement by post-dodge penalty
            StartCoroutine(AfterDodgeDelay(afterDodgeDelay));
            // Set isDodging to false
            isDodging = false;
        }
    }

    void FaceDirection(float inputH, float inputV)
    {
        // If we are currently sensing any input
        if ((inputH != 0 || inputV != 0) && !isDodging)
        {
            // Set our Direction variable as Motion (i.e. the last direction we travelled in based on inputs) normalised into -1, 0 or 1
            direction = motion.normalized;
        }

        // As long as our x and y direction aren't both 0
        if (!(direction.x == 0 && direction.y == 0))
        {
            // Set our animator floats for x and y
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);

            // Face left or right depending on which way we x direction we last input
            rend.flipX = direction.x < 0;
        }
    }

    #region Enumerators
    IEnumerator PreDodgeDelay(float delay)
    {
        CanMove = false;
        motion = Vector2.zero;
        anim.SetTrigger("DodgeRoll");
        yield return new WaitForSeconds(delay);
        CanMove = true;
    }

    IEnumerator AfterDodgeDelay(float delay)
    {
        CanMove = false;
        direction = dashDirection;
        yield return new WaitForSeconds(delay);
        CanMove = true;
    }
    #endregion
}