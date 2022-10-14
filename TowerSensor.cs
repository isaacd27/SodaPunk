using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSensor : MonoBehaviour
{
    float radius = 360;

    public float angle;

   EnemyController trueTarget;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public int prevpriority;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVRoutine());
        // obstructionMask.
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
    // Update is called once per frame
    void Update()
    {

    }

    private void FieldOfViewCheck()
    {
        Collider[] rangechecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        EnemyController[] objectist = new EnemyController[rangechecks.Length];

        for (int i = 0; i < rangechecks.Length - 1; i++)
        {
            foreach (Collider c in rangechecks)
            {
                if (c.gameObject.GetComponent<EnemyController>() != null)
                {
                    objectist[i] = c.gameObject.GetComponent<EnemyController>();
                }
                else
                {
                    objectist[i] = null;
                }
            }
        }
        prevpriority = 0;
        EnemyController target = null;

        for (int i = 0; i < rangechecks.Length - 1; i++)
        {
            if (objectist[i] != null)
            {
                
                    //prevpriority = objectist[i].GetPriority();
                    target = objectist[i];
                
            }
        }

        //Debug.Log(target.gameObject + " " + prevpriority);
        setTar(target);
    }

    void setTar(EnemyController o)
    {
        trueTarget = o;
    }

    public EnemyController getTar()
    {
        return trueTarget;
    }


}
