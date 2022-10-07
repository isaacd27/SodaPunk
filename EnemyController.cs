using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public TestCone Targ;

    Object target = null;

    public float movespeed;
    public float rotationSpeed;

    public float maxtime;
    float time;

    int state;
    const int STATE_WALK = 0;
    const int STATE_ATTACK = 1;

    public int Maxhp;
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = Maxhp;
        Targ = this.GetComponent<TestCone>();
        if (Targ == null)
        {
            Debug.LogError("no Testcone!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {

            case STATE_WALK:
                check();
                break;
            case STATE_ATTACK:
                movement();
               // Debug.Log(state);
                break;
            default:
                check();
               // Debug.Log(state);
                break;
        }
       


        Debug.Log(state);





    }

    public void damage(int Damage)
    {
        hp -= Damage;
    }

    void check()
    {
        if (Targ.getTar() != null)
        {
            target = Targ.getTar();
        }
        else
        {
            state = STATE_WALK;
        }

        if (target != null)
        {
            time += Time.deltaTime;
            if (time > maxtime)
            {
                state = STATE_ATTACK;
            }
        }
    }

    void movement()
    {
        GameObject Target = target.gameObject;
        Quaternion targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        float str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
        transform.Translate(0, 0, movespeed * Time.deltaTime);

        if (Targ.getTar() == null)
        {
            state = STATE_WALK;
            time = 0;
        }
    }
}
