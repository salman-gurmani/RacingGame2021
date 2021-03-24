using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public GameObject arrowObj;

    private void Start()
    {

        if (Toolbox.GameplayScript)
            Toolbox.GameplayScript.carArrowScript = this;
        else
            Status(false);
    }

    private void Update()
    {
        if (target)
            this.transform.LookAt(target);
    }

    public void SetTarget(Transform _val) {

        target = _val;
    }

    public void Status(bool _val) {

        this.arrowObj.SetActive(_val);
    }
}
