using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PixelCrew.Creatures;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetFollowComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vCamera = GetComponent<CinemachineVirtualCamera>();
        vCamera.Follow = FindObjectOfType<Hero>().transform;
    }


}
