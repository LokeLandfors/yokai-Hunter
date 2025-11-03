using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : Interactable
{
    Rigidbody2D rb;
    Vector3 resetPos;
    Quaternion resetRot;
    private float times = 1f;
    private bool timerRunning = false;

    Renderer rend;

    [SerializeField] Gradient gradient;

    Color currentColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        currentColor = gradient.Evaluate(0);
        resetPos = transform.position;
        resetRot = transform.rotation;  
        rb = GetComponent<Rigidbody2D>();
        rend.material.color = Color.green;
    }

    void Update()
    {
        currentColor = gradient.Evaluate(1- (times / 1f));
        rend.material.color = currentColor;
        if (timerRunning)
        {
            times -= Time.deltaTime;
            if (times <= 0)
            {
                times = 0;
                timerRunning = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
               
            }
        }
    }

    public void Reset()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = resetPos;
        transform.rotation = resetRot;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Renderer>().material.color = Color.green;

        times = 2f;
        timerRunning = false;
    }

    public override void Interact(PlayerMovement player)
    {
        if (!timerRunning && times > 0)
        {
            timerRunning = true; 
        }
    }
}
