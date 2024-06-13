using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public Point pointA;
    public Point pointB;

    private float moveDuration = 1.0f; 
    private float moveTimer = 0f;      
    private Transform objectToMove;    
    private Vector3 startPosition;     
    private Vector3 targetPosition;    

    private void OnEnable()
    {
        BaseInteractableObj.OnObjectRemoved += HandleObjectRemoved;
    }

    private void OnDisable()
    {
        BaseInteractableObj.OnObjectRemoved -= HandleObjectRemoved;
    }

    private void HandleObjectRemoved(Point point)
    {
        if (point == pointB)
        {
            UpdatePoints();
        }
    }

    void Update()
    {
        UpdatePoints();
        UpdateMovement();
    }

    void UpdatePoints()
    {
        if (!pointB.isOccupied && pointA.isOccupied)
        {
            InitiateMoveFromAToB();
        }
    }

    void InitiateMoveFromAToB()
    {
        if (pointA.transform.childCount > 0)
        {
            objectToMove = pointA.transform.GetChild(0);
            startPosition = objectToMove.position;
            targetPosition = pointB.transform.position;
            moveTimer = 0f;
        }
    }

    void UpdateMovement()
    {
        if (objectToMove != null)
        {
            moveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(moveTimer / moveDuration);
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1f)
            {
                FinishMoveFromAToB();
            }
        }
    }

    void FinishMoveFromAToB()
    {
        objectToMove.SetParent(pointB.transform);
        pointA.isOccupied = false;
        pointB.isOccupied = true;
        var interactableObj = objectToMove.GetComponent<BaseInteractableObj>();
        if (interactableObj != null)
        {
            interactableObj.SetCurrentPoint(pointB);
        }
        objectToMove = null;
    }
}