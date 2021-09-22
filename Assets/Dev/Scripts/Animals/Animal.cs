using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ScriptableObjectArchitecture;
using UnityEngine.VFX;

public class Animal : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private Transform[] points;

    [SerializeField]
    private BoolVariable _isAppeared;

    [SerializeField]
    private GameObject _DebugVFX;

    #endregion

    #region Private

    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 _destination;
    private List<Plants> _infestationPoint = new List<Plants>();

    #endregion

    #region Unity API


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
        _isAppeared.Value = true;
    }




    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
            GotoNextPoint();

       
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _destination);
 
    }
    #endregion

    #region Private
    void GotoNextPoint()
    {
        if (_infestationPoint.Count > 0)
        {
            _destination = _infestationPoint[0].transform.position;
            agent.destination = _destination;
            Debug.Log(agent.destination + " infesté");
            if (Vector3.Distance(transform.position, _destination) < 0.5f)
            {
                    _infestationPoint[0].setInfested(false);
                    _infestationPoint.Remove(_infestationPoint[0]);
                agent.speed /= 2;
                GameObject wt = Instantiate(_DebugVFX, transform.position, Quaternion.identity);
                wt.GetComponent<VisualEffect>().SendEvent("DeBugged");
                Destroy(wt, 5f);
            }
        }
        else
        {
            if (points.Length == 0)
                return;

            destPoint = Random.Range(0,points.Length) % points.Length;
            agent.destination = points[destPoint].position;
            Debug.Log(agent.destination + " patrouille");

        }

    }

    public void AddDestination(Plants p)
    {
        _infestationPoint.Add(p);
        agent.speed *= 2;
        GotoNextPoint();
    }
    #endregion
}
