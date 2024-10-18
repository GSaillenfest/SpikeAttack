using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffects : MonoBehaviour
{
    public void ShowSelectable(ArrayList playerArray)
    {
        //if isSelectable
        //add gloom effect around objects
    }

    public void ShowUnselectable(ArrayList playerArray)
    {
        //if !isSelectable
        //remove all effect arount object
    }

    public void ShowSelected(VolleyPlayer player)
    {
        //add liner aroung object
    }    
    
    public void ShowUnselected()
    {
        //add liner aroung object
    }

    public void ShowSelectedAction(GameObject action)
    {
        //add effect around action
    }    
    
    public void ShowUnselectedAction(GameObject action)
    {
        //add effect around action
    }
}
