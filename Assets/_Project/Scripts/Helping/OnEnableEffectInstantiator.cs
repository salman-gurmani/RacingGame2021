using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Effect { 
    
    [SerializeField] public GameObject effectObj;
    [SerializeField] public bool initAtParentPos = true;
}


public class OnEnableEffectInstantiator : MonoBehaviour
{
    [SerializeField] public Effect [] effect;
    [SerializeField] public float time = 1;
    void Start()
    {
        StartCoroutine(InitEffect());                
    }

    IEnumerator InitEffect() {

        yield return new WaitForSeconds(time);

        for (int i = 0; i < effect.Length; i++)
        {
            if (effect[i].initAtParentPos) {
                Instantiate(effect[i].effectObj, this.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(effect[i].effectObj, Vector3.zero, Quaternion.identity);
            }
            
        }
    }

  
}
