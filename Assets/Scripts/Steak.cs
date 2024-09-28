using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : Food
{
    [SerializeField] private int numberOfMoves;
    [SerializeField] private int currentMoves;
    [SerializeField] private float scaleChangeSpeed;
    [SerializeField] private Vector3 startingScale;
    [SerializeField] private float scalar;
    [SerializeField] private Vector3 endingScale;
    public override void Awake()
    {
        base.Awake();
        isLifting = false;
        isFalling = false;
        moveSpeed = 16;
        moveDistance = 2;
        numberOfMoves = 2;
        currentMoves = 0;
        scalar = 1.25f;
        startingPosition = transform.position;
        startingScale = transform.localScale;
        endingScale = new Vector3(startingScale.x * scalar, 1, startingScale.z * scalar);
        scaleChangeSpeed = 2;
        liftToLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLifting && currentMoves < numberOfMoves)
        {
            if (Vector3.Distance(transform.position, liftToLocation) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, liftToLocation, moveSpeed * Time.deltaTime);
                transform.localScale = Vector3.MoveTowards(transform.localScale, endingScale, scaleChangeSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = liftToLocation;
                transform.localScale = endingScale;
                isLifting = false;
                isFalling = true;
            }
        }
        if (isFalling)
        {
            if (Vector3.Distance(transform.position, startingPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, moveSpeed * Time.deltaTime);
                transform.localScale = Vector3.MoveTowards(transform.localScale, startingScale, scaleChangeSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = startingPosition;
                transform.localScale = startingScale;
                isFalling = false;
                currentMoves++;
                PlaySlamSound();
                if (currentMoves < numberOfMoves)
                {
                    isLifting = true;
                }
            }
        }        
    }
    public override void Reveal()
    {
        isLifting = true;
    }

    public override void PlaySound()
    {
        foodSoundSource.pitch = 0.75f;
    }

    private void PlaySlamSound()
    {
        base.PlaySound();
    }
}
