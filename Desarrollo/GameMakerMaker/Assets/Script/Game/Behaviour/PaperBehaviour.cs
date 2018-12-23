using UnityEngine;
using System.Collections;

public class PaperBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        PropBehaviour p = other.GetComponent<PropBehaviour>();

        if(p == null)
        {
            p = other.transform.GetComponentInParent<PropBehaviour>();
        }


        if(p != null)
        {
            GameManager.Instance.StoreProp(p.gameObject);
            LifeController.Instance.Lifes--;
        }
        
    }
}
