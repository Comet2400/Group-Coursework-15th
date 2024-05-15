using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5f; // Distance to dash
    public float dashDuration = 0.2f; // Duration of dash in seconds
    public float dashCooldown = 2f; // Cooldown period for dash in seconds

    private float lastDashTime; // Time when the last dash occurred

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AttemptDash();
        }
    }

    public void AttemptDash()
    {
        if (Time.time - lastDashTime > dashCooldown)
        {
            DashAction();
        }
    }

    void DashAction()
    {
        // Perform the dash
        Vector3 dashDirection = transform.right; // Dash in the direction player is facing (adjust as needed)
        Vector3 dashEndPosition = transform.position + dashDirection * dashDistance;
        StartCoroutine(PerformDash(dashEndPosition));

        // Update last dash time
        lastDashTime = Time.time;
    }

    IEnumerator PerformDash(Vector3 dashEndPosition)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            transform.position = Vector3.Lerp(startPosition, dashEndPosition, t);
            yield return null;
        }
    }
}

