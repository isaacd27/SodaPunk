using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCone : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    Object trueTarget;

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
        Object[] objectist = new Object[rangechecks.Length];

        for (int i = 0; i < rangechecks.Length - 1; i++)
        {
            foreach (Collider c in rangechecks)
            {
                if (c.gameObject.GetComponent<Object>() != null)
                {
                    objectist[i] = c.gameObject.GetComponent<Object>();
                }
                else
                {
                    objectist[i] = null;
                }
            }
        }
        prevpriority = 0;
        Object target = null;

        for (int i = 0; i < rangechecks.Length - 1; i++)
        {
            if (objectist[i] != null)
            {
                if (objectist[i].GetPriority() > prevpriority)
                {
                    prevpriority = objectist[i].GetPriority();
                    target = objectist[i];
                }
            }
        }

        //Debug.Log(target.gameObject + " " + prevpriority);
        setTar(target);
    }

    void setTar(Object o)
    {
        trueTarget = o;
    }

    public Object getTar()
    {
        return trueTarget;
    }

    public void debug(Object target)
    {
        Debug.Log(target.gameObject + " " + prevpriority);
    }

}
