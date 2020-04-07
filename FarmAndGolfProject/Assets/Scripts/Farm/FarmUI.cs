using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmUI : MonoBehaviour
{
    public GameObject farmOp;
    public FarmOperation fop;
    public PastureOP pop;

    // Start is called before the first frame update
    void Start()
    {
        farmOp.SetActive(false);
        if(fop==null)
            fop = GameObject.FindGameObjectWithTag("Player").GetComponent<FarmOperation>();
        if (pop == null)
            pop = GameObject.FindGameObjectWithTag("Player").GetComponent<PastureOP>();
    }

    public void FarmOpUI()
    {
        farmOp.SetActive(!farmOp.activeSelf);
        if (!farmOp.activeSelf)
        {
            if(fop!=null)
                fop.SetReset();
            if (pop != null)
                pop.SetReset();
        }
    }
}
