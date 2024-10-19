using UnityEngine;

public class PlayerDeckOnField : MonoBehaviour
{
    [SerializeField]
    GameObject[] deckSlots;

    // Start is called before the first frame update
    void Start()
    {
        deckSlots = new GameObject[6];
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
            if (i == 5)
            {
                //a la place du slot 1
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[1].transform);
            }
            else
            {
                //à la place du slot i + 1
                Debug.Log(i);
                deckSlots[i].transform.GetChild(0).SetParent(deckSlots[i + 1].transform);
            }
        }
        // slot 2 = tempPlayerSlot
        tempPlayerSlot.transform.GetChild(0).SetParent(deckSlots[2].transform);

        RefreshDeck();
    }
}
