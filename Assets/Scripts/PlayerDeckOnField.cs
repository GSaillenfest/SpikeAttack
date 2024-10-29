using UnityEngine;

public class PlayerDeckOnField : MonoBehaviour
{
    [SerializeField]
    public GameObject[] deckSlots;

    // Start is called before the first frame update
    void Start()
    {
        //deckSlots = new GameObject[6];
        RefreshDeck();
    }

    void RefreshDeck()
    {
        VolleyPlayer[] childList = gameObject.GetComponentsInChildren<VolleyPlayer>();
    }


    public void RotatePlayerCards()
    {
        GameObject tempPlayerSlot = deckSlots[1];

        for (int i = deckSlots.Length - 1; i > 1; i--)
        {
            if (i == deckSlots.Length - 1)
            {
                //a la place du slot 1
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[1].transform, false);
            }
            else
            {
                //à la place du slot i + 1
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[i + 1].transform, false);
            }
        }
        // slot 2 = tempPlayerSlot
        tempPlayerSlot.transform.GetChild(0).SetParent(deckSlots[2].transform, false);

        RefreshDeck();
    }
}
