using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public Sprite pared1, pared2, suelo1, suelo2, suelo3, suelo4;
    private void Start()
    {
        int range = Random.Range(1, 4);
        int range1 = Random.Range(1, 5);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if(transform.parent.name == "Blocks")
        {
            if (range == 1)
            {
                renderer.sprite = pared1;
            }
            else
            {
                renderer.sprite = pared2;
            }
        }
        else
        {
            if (range1 == 1)
            {
                renderer.sprite = suelo1;
            }
            else if (range1 == 2)
            {
                renderer.sprite = suelo2;
            }
            else if (range1 == 3)
            {
                renderer.sprite = suelo3;
            }
            else
            {
                renderer.sprite = suelo4;
            }
        }
    }

}
