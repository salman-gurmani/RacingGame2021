using UnityEngine;

public class OnTrigger_EventInstantiator : MonoBehaviour
{
    public enum EventType { 
    
        INSTANTIATE_OBJECT,
        LEVEL_COMPLETE

    }
    public EventType type;

    [Header("INSTANTIATE_OBJECT")]
    public GameObject obj;
    public Transform init_point;

    public void InitEvent() {

        switch (type) {

            case EventType.INSTANTIATE_OBJECT:

                Instantiate(obj, init_point.position, init_point.rotation);
                break;

            case EventType.LEVEL_COMPLETE:
                Toolbox.GameplayScript.LevelCompleteHandling();

                break;
        }

        Destroy(this.gameObject);
    }    
}
