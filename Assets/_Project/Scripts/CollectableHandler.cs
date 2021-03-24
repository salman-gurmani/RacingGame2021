using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHandler : MonoBehaviour
{
    private bool startAnim = false;

    public enum Type { 
    
        COIN,
        FLARE,
        SHIELD,
        BOOST,
        REPAIR

    }public Type type = Type.COIN;
    
    public bool moveToRandomPoint = false;
    private Vector3 randomPoint;

    private void Start()
    {
        if (moveToRandomPoint) {

            randomPoint = this.transform.position + Random.insideUnitSphere * 5;
        }        
    }

    private void Update()
    {
        if (startAnim)
        {
            CoinGetAnimation();    
        }

        if (moveToRandomPoint)
        {
            RandomPointAnimation();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerWing")) {



            switch (type)
            {

                case Type.COIN:

                    startAnim = true;
                    this.GetComponent<SizeAnimation>().enabled = false;
                    Toolbox.GameplayScript.IncrementGoldCoins(1);
                    this.GetComponent<TweenMovementToTransform>().enabled = false;

                    Destroy(this.gameObject, 1);

                    break;

                case Type.BOOST:

                    Destroy(this.gameObject);

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
    }

    private void CoinGetAnimation()
    {
        this.transform.position += new Vector3(-1.4f, 1, 0) * Time.deltaTime * 30;

        if (this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - 0.02f,
                                                this.transform.localScale.y - 0.02f,
                                                    this.transform.localScale.z - 0.02f);

        }
    }

    private void RandomPointAnimation()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, randomPoint, 0.05f);

        if (Vector3.Distance(this.transform.position, randomPoint) < 0.2f)
        {
            moveToRandomPoint = false;
        }
    }

}
