using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField, Range(0.001f, 1f)] private float amount = 0.002f;
    [SerializeField, Range(1f, 30f)] private float frequency = 10f;
    [SerializeField, Range(1f, 100f)] private float smoothness = 10f;

    Vector3 startPos;

    private Ak ak;

    private Movement movement;



    private void Awake()
    {
        ak = GetComponentInChildren<Ak>();
        movement = FindObjectOfType<Movement>();
    }


    private void Start()
    {
        startPos = transform.localPosition;
    }


    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadbob();
        SetAmountValue();
    }



    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if (inputMagnitude > 0)
        {
            StartHeadbob();
        }
    }

    private Vector3 StartHeadbob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smoothness * Time.deltaTime) / 1.5f;
        // pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smoothness * Time.deltaTime) / 2;
        transform.localPosition += pos;
        return pos;
    }

    private void StopHeadbob()
    {
        if (transform.localPosition == startPos)
        {
            return;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 0.1f * Time.deltaTime);
    }

    public void SetAmountValue()
    {
        if (ak.isAiming == true)
        {
            amount = 0f;
            frequency = 0f;
            smoothness = 0f;
        }
        if (ak.isAiming == false)
        {
            amount = 0.03f;
            frequency = 10f;
            smoothness = 10f;
        }

        if (movement.sprint)
        {
            amount = 0.05f;
            frequency = 10f;
            smoothness = 10f;
        }
        if (!movement.sprint)
        {
            amount = 0.03f;        
        }
    }



}
