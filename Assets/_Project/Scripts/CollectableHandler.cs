using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHandler : MonoBehaviour
{

    public enum Type {

        COIN,
        FLARE,
        SHIELD,
        BOOST,
        REPAIR

    } public Type type = Type.COIN;

    public GameObject obj;

    public void OnCollect() {

        switch (type)
        {

            case Type.COIN:

                this.GetComponent<SizeAnimation>().enabled = false;
                Toolbox.GameplayScript.IncrementGoldCoins(1);
                this.GetComponent<TweenMovementToTransform>().enabled = false;

                Destroy(this.gameObject, 1);

                break;

            case Type.BOOST:

                obj.SetActive(false);
                Invoke("EnableObj", 5);
                break;

            case Type.FLARE:

                Destroy(this.gameObject);

                break;

            case Type.REPAIR:

                Destroy(this.gameObject);

                break;

            case Type.SHIELD:

                Destroy(this.gameObject);

                break;
        }
    }

    void EnableObj() {

        obj.SetActive(true);
    }
}
