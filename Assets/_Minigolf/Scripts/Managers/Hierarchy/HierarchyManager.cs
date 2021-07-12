using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigolf
{
  public class HierarchyManager : MonoBehaviour
  {
    private int headerAmount;
    [SerializeField] private List<HierarchyHeader> headerGOs;
    [SerializeField] private List<Transform> headerChildrenGOs;

    private void Start()
    {
      AssignHeaderGOs();
      DetachAllHeaders();
    }

    private void AssignHeaderGOs()
    {
      headerGOs = new List<HierarchyHeader>();
      headerGOs = transform.GetComponentsInChildren<HierarchyHeader>().ToList();
      headerAmount = headerGOs.Count;
    }

    private void DetachAllHeaders()
    {
      for (int i = 0; i < headerAmount; i++)
      {
        DetachHeaderWithChildren(i);
      }
    }

    private void DetachHeaderWithChildren(int index)
    {
      AssignHeaderChildrenGOs(index);

      for (int i = 0; i < headerChildrenGOs.Count; i++)
      {
        if (headerChildrenGOs[i].parent != headerGOs[index].transform &&
                        headerChildrenGOs[i].parent != this.transform) continue;

        headerChildrenGOs[i].parent = null;
        headerChildrenGOs[i].SetAsLastSibling();
      }
    }

    private void AssignHeaderChildrenGOs(int index)
    {
      headerChildrenGOs = new List<Transform>();
      headerChildrenGOs = headerGOs[index].GetComponentsInChildren<Transform>().ToList(); // First element is the header
    }
  }
}