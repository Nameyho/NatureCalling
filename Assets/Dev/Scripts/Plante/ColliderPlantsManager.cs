using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlantsManager : MonoBehaviour
{

    private SphereCollider[] _sp;
public void DisableEffectCollider()
    {

        for (int i = 0; i < _sp.Length; i++)
        {
                    Debug.Log(_sp[i].tag);
            if (_sp[i].CompareTag("EffectZone"))
            {
                _sp[i].enabled = false;
            }
        }
    }


    public void EnableEffectCollider()
    {
        for (int i = 0; i < _sp.Length; i++)
        {
            if (_sp[i].CompareTag("EffectZone"))
            {
                _sp[i].enabled = true;
            }
        }
    }

    private void Awake()
    {
       _sp =  GetComponents<SphereCollider>();
        
    }
}
