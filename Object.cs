using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public int priority;

    public bool calPriority;

    public int GetPriority()
    {
        return priority;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (calPriority)
        {
            calculate();
        }
    }

    void calculate()
    {

    }

}
