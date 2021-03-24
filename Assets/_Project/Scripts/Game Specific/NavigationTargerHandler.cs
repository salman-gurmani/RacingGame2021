using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTargerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       FindObjectOfType<InsaneSystems.RoadNavigator.Navigator>().SetTargetPoint(this.gameObject.transform.position); 
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
