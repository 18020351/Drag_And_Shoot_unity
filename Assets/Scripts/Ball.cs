using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool isPressed;
    private Rigidbody2D ballRb;
    private Rigidbody2D slingRb;
    private SpringJoint2D ballSj;
    private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;
    private float releaseDelay;
    private float maxDragDistance = 5f;
    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
        ballSj = GetComponent<SpringJoint2D>();
        slingRb = ballSj.connectedBody;
        lineRenderer = GetComponent<LineRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        releaseDelay = 1 / (ballSj.frequency * 4);
        lineRenderer.enabled = false;
        trailRenderer.enabled = false;
    }
    private void Update()
    {
        if (isPressed)
        {
            DragBall();
        }
    }
    private void OnMouseDown()
    {
        isPressed = true;
        ballRb.isKinematic = true;
        lineRenderer.enabled = true;
    }
    private void OnMouseUp()
    {
        isPressed = false;
        ballRb.isKinematic = false;
        StartCoroutine(Release());
        lineRenderer.enabled = false;
    }
    private void DragBall()
    {
        SetLineRenderPosition();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousePosition, slingRb.position);
        // if (distance > maxDragDistance)
        // {
        //     Vector2 direction = (mousePosition - slingRb.position).normalized;
        //     ballRb.position = slingRb.position + direction * maxDragDistance;
        // }
        // else
        // {
        //     ballRb.position = mousePosition;
        // }
        Vector2 direction = (mousePosition - slingRb.position).normalized;
        ballRb.position = (distance > maxDragDistance) ? (slingRb.position + direction * maxDragDistance) : mousePosition;
    }
    private void SetLineRenderPosition()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = ballRb.position;
        positions[1] = slingRb.position;
        lineRenderer.SetPositions(positions);
    }
    private IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseDelay);
        ballSj.enabled = false;
        trailRenderer.enabled = true;
    }
}
