using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private Transform[] points;

    #endregion

    #region Private

    private int destPoint = 0;
    private NavMeshAgent agent;

    #endregion

    #region Unity API


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }




    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    #endregion

    #region Private
    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        agent.destination = points[destPoint].position;
        destPoint = Random.Range(0,points.Length) % points.Length;
    }
    #endregion
}
