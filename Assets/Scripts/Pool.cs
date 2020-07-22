using System.Collections;
using UnityEngine;


public class Pool
{
    private GameObject[] pool;
    private int nextAvailable = 0;

    public Pool(GameObject prefab, int length)
    {
        pool = new GameObject[length];

        for (int i = 0; i < length; ++i)
        {
            pool[i] = GameObject.Instantiate(prefab);
            pool[i].SetActive(false); // TODO: consider loading resource so this is never active.  OR find some way to reliably ensure prefabs are inactive
        }
    }

    public GameObject Get()
    {
        for (int count = pool.Length; count > 0; count--)
        {
            GameObject next = pool[nextAvailable];
            nextAvailable = (nextAvailable + 1) % pool.Length;

            //Debug.Log("  Get[" + nextAvailable + "]: " + (next == null ? "NEXT-NULL" : "NEXT-OK") + " : Active=" + (next.activeSelf ? "Y" : "N"));
            if (!next.activeSelf)
                return next;
        }

        return null;
    }

    public GameObject Activate(Vector3 position, Quaternion rotation)
    {
        GameObject item = Get();
        //Debug.Log("Pool.Activate: item=" + (item == null ? "FAIL" : "PASS") + "  Pool length is " + pool.Length);

        if (item != null)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.SetActive(true);
            return item;
        }

        return null;
    }

}


