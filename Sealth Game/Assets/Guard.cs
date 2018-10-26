using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform pathHolder;
    public float movementSpeed = 5;
    public float rotateSpeed = 90;
    public float waitTimeInWaypoints = .2f;

	void Start ()
    {
        Vector3[] wayPoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = pathHolder.GetChild(i).transform.position;
        }
        StartCoroutine(Patrol(wayPoints));
	}

    IEnumerator Patrol(Vector3[] wayPoints)
    {
        transform.position = wayPoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = wayPoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            yield return StartCoroutine(MoveTo(targetWaypoint));
            targetWaypointIndex = (targetWaypointIndex + 1) % wayPoints.Length;
            targetWaypoint = wayPoints[targetWaypointIndex];

            yield return new WaitForSeconds(waitTimeInWaypoints);
            yield return StartCoroutine(FaceTo(targetWaypoint));
        }

        
    }
    IEnumerator FaceTo(Vector3 positionToFace)
    {
        Vector3 direction = (positionToFace - transform.position).normalized;
        float lookAngle = 90 - (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
        Debug.Log(lookAngle.ToString());

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y,lookAngle)) > 0.1f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, lookAngle, rotateSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
    
    IEnumerator MoveTo(Vector3 endPos)
    {
        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
    void OnDrawGizmos()
    {
        Vector3 startPos = pathHolder.GetChild(0).transform.position;
        Vector3 prevPos = startPos;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .2f);
            Gizmos.DrawLine(prevPos, waypoint.position);

            prevPos = waypoint.position;
        }
        Gizmos.DrawLine(prevPos, startPos);
    }
}
