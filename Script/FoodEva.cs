using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEva : MonoBehaviour
{
    private string[] hamburger = new string[3] {"Bread_Base", "Meat", "Bread_Top"};
    
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "food")
        {
            //other.SetParent(transform);     // Ingridients becomes the child of the bag
            Evaluate();
        }
    }
    private void Evaluate()
    {
        float nIngridient = transform.childCount;
        if (nIngridient != hamburger.Length)
        {
            // doesn't coincide
        }
        if (nIngridient == hamburger.Length)
        {
            // coincide
            for (int i = 0; i < nIngridient; i++)
            {
                GameObject childObj = this.gameObject.transform.GetChild(i).gameObject;
                string name = childObj.name; 
                if(name == hamburger[i])
                {
                    // correct ingridient, correct order. Think on how the order will be correctly evaluated
                }
            }
        }
    }
}
