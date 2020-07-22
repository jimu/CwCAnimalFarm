using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// a pooled prefab that automaticall 1) sets text value and 2) deactivates itself after 1.0 seconds
// animation should play automatically
public class PointsGizmo : MonoBehaviour
{
    // TODO: Static method is sloppy, replace with singleton or merge text feater into Pool or implement this in
    static public GameObject Activate(Pool pool, int value, Vector3 position)
    {
        //Init();
        // use Get instead of Activate???
        GameObject item = pool.Activate(position, Quaternion.identity);

        if (pool == null)
            Debug.LogError("PointsGizmo Pool Null");
        if (item == null)
            Debug.LogError("PointsGizmo Pool Empty [" + value + "]");
        item.GetComponent<Text>().text = "+" + value.ToString();
        return item;
    }

    /*
    static private void Init()
    {
        if (pool == null)
            pool = new Pool(prefab, 10);
    }
    */

    private void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
