using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour
{
    [SerializeField] private GameObject starsRow;

    public void SetStars(int value)
    {
        for (int i = 0; i < starsRow.transform.childCount; i++)
        {
            var imageMask = starsRow.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Mask>();
            if (i * 2 < value - 1) //full star
            {
                imageMask.gameObject.SetActive(true);
                imageMask.enabled = false;
            }
            else if (i * 2 == value - 1) //half star
            {
                imageMask.gameObject.SetActive(true);
                imageMask.enabled = true;
            }
            else //no star
            {
                imageMask.gameObject.SetActive(false);
            }
        }
    }
}
