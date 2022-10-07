using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCone : MonoBehaviour
{
    // Start is called before the first frame update
  
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    public int prevpriority;

    public float rotationSpeed;
    public float movespeed;

     int state;

    const int STATE_WALK = 0;
    const int STATE_ATTACK = 1;

    //GameObject[] Objects = new GameObject[];

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
       // StartCoroutine(FOVRoutine());
       // obstructionMask.
    }
    public void Update()
    {
       
        switch (state)
        {
           
            case STATE_WALK:
                FieldOfViewCheck();
                break;
            case STATE_ATTACK:
                chase();
                Debug.Log(state);
                break;
            default:
                FieldOfViewCheck();
                Debug.Log(state);
                break;
        }
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    void chase()
    {

        GameObject target = gettarget();
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        float str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
        transform.Translate(0, 0, movespeed * Time.deltaTime);


        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);


        RaycastHit obs;
        Ray ray = new Ray(transform.position, directionToTarget);
        if (!Physics.Raycast(ray, out obs, distanceToTarget, obstructionMask))
        {
            Object o = obs.transform.gameObject.GetComponent<Object>();
            if (o != null)
            {
                int i = o.GetPriority();
                if (i > prevpriority)
                {
                    prevpriority = i;
                    transform.LookAt(o.gameObject.transform);
                    state = STATE_ATTACK;
                }
            }
            else
            {
                state = STATE_WALK;
            }
        }
    }

    GameObject tar;

    public GameObject gettarget()
    {
        return tar;
    }

    public void settarget(GameObject t)
    {
        tar = t;
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit obs;
                Ray ray = new Ray(transform.position, directionToTarget);
                Debug.DrawRay(transform.position, directionToTarget,Color.red);
                if (!Physics.Raycast(ray, out obs, distanceToTarget, obstructionMask))
                {
                  /*  Object o = obs.transform.gameObject.GetComponent<Object>();
                    if (o != null)
                    {
                        int i = o.GetPriority();
                        if ( i > prevpriority)
                        {
                            prevpriority = i;
                            transform.LookAt(o.gameObject.transform);
                            state = STATE_ATTACK;
                            settarget(o.gameObject);
                            Debug.Log(o.gameObject);
                        }
                    }
                    else
                    {
                        prevpriority = 0;
                    } */
                    canSeePlayer = true;

                }
                else
                    canSeePlayer = false;
               
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}

