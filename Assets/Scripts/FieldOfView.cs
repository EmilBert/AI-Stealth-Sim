using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public enum GuardStates
    {
        DEFAULT,
        SUSPICIOUS,
        ALERTED
    }
    public float viewRadius;
    [Range(0,100)]
    public float susPercentage;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
 
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> suspicousTargets = new List<Transform>();
    
    private Agent agent;
    private Transform currentTarget;
    private GuardStates state = GuardStates.DEFAULT;  

    void Start(){
        agent = GetComponent<Agent>();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }
    
    IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            FindSuspiciousTargets();
        }
    }

    public Transform GetCurrentTarget(){
        if(currentTarget) return currentTarget;
        return null;
    }

    public GuardStates GetState(){
        return state;
    }

    void FindVisibleTargets(){
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, (susPercentage/100)*viewRadius, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++){
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2){
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)){
                    visibleTargets.Add(target);
                    currentTarget = target;
                    state = GuardStates.ALERTED;
                }
            }
        }
    }
    void FindSuspiciousTargets(){
        suspicousTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++){
            Transform target = targetsInViewRadius[i].transform;
            if(visibleTargets.Contains(target)){
                continue;
            }
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2){
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)){
                    suspicousTargets.Add(target);
                    currentTarget = target;
                    state = GuardStates.SUSPICIOUS;
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

