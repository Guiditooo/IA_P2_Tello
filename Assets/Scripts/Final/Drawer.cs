using UnityEngine;

[System.Serializable]
public struct Drawer
{
    public const int MAX_OBJS_PER_DRAWCALL = 1000;

    public GameObject prefab;
    public Mesh prefabMesh;
    public Material prefabMaterial;
    public Vector3 prefabScale;
}
