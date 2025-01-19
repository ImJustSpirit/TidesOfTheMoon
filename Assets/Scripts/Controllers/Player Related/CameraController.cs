using System;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = System.Numerics.Vector3;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Camera cam;
    
    void Awake()
    {
        cam = Camera.main;
    }
}
