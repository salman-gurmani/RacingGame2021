using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerColliderHandler : MonoBehaviour
{
    public LayerMask mask;

    private void Start()
    {
        BoxCollider col = this.GetComponent<BoxCollider>();
    }
}
