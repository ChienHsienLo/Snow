using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintSystem : MonoBehaviour
{
    [SerializeField] Transform leftFootPoint;
    [SerializeField] Transform rightFootPoint;

    [SerializeField] Transform poolRoot;
    [SerializeField] int footPrintCount;
    [SerializeField] GameObject footPrintPrefab;
    
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        for(int i = 0; i < footPrintCount; i++) 
        {
            GameObject prefab = Instantiate(footPrintPrefab, poolRoot, false);
            footprints.Enqueue(prefab);

            prefab.SetActive(false);
        }


        SetRightFootprint();
        SetLeftFootprint();
    }

    private void OnFootstepL(AnimationEvent animationEvent)
    {
        SetLeftFootprint();
    }

    private void OnFootstepR(AnimationEvent animationEvent)
    {
        SetRightFootprint();
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        SetRightFootprint();
        SetLeftFootprint();
    }

    void Raycast(Transform trans)
    {
        Ray ray = new Ray(trans.position + new Vector3(0.0f, 0.2f, 0.0f), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.5f, layerMask))
        {
            Vector3 hitPosition = hitInfo.point;
            
            var fp = GetFootprint();
            fp.transform.position = hitPosition;
            fp.transform.rotation = trans.rotation;
            Vector3 euler = trans.eulerAngles;
            euler.x = 0.0f;
            euler.z = 0.0f;
            fp.transform.eulerAngles = euler;

            fp.SetActive(true);
        }
    }

    GameObject GetFootprint()
    {
        GameObject prefab = footprints.Dequeue();
        footprints.Enqueue(prefab);

        return prefab;
    }

    void SetRightFootprint()
    {
        Raycast(rightFootPoint);
    }

    void SetLeftFootprint()
    {
        Raycast(leftFootPoint);
    }




    Queue<GameObject> footprints = new Queue<GameObject>();
}
