using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Controlar cada tipo de animal
    //Carroñero
    //Carnívoro
    //Herbívoro
    //Agregarle cada componente a los animales

    uint gameEntity = 0;

    uint gridEntity = 0;

    [SerializeField] private int herbivoreCount = 5;
    [SerializeField] private int carnivoreCount = 5;
    [SerializeField] private int scavengerCount = 5;

    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;

    private List<Herbivore> herbivores;
    private List<Carnivore> carnivores;
    private List<Scavenger> scavengers;

    private Dictionary<Type, IList> animalLists;

    private void Start()
    {
        ECSManager.Init();

        herbivores = new List<Herbivore>();
        carnivores = new List<Carnivore>();
        scavengers = new List<Scavenger>();

        animalLists = new Dictionary<Type, IList>();

        animalLists[typeof(Herbivore)] = herbivores;
        animalLists[typeof(Carnivore)] = carnivores;
        animalLists[typeof(Scavenger)] = scavengers;

        CreateGrid();
        SetUpGame();
    }

    private void CreateGrid()
    {
        gridEntity = ECSManager.CreateEntity();
        ECSManager.AddComponent<GridBoundsComponent>(gridEntity, new GridBoundsComponent(gridWidth, gridHeight));
    }

    private void SetUpGame()
    {
        gameEntity = ECSManager.CreateEntity();
        ECSManager.AddComponent<GameComponent>(gameEntity, new GameComponent(herbivoreCount, carnivoreCount, scavengerCount));

        CreateAnimals<Herbivore>(herbivoreCount);
        CreateAnimals<Carnivore>(carnivoreCount);
        CreateAnimals<Scavenger>(scavengerCount);
    }

    private void CreateAnimals<AnimalType>(int count) where AnimalType : AnimalBase, new()
    {
        if (animalLists.TryGetValue(typeof(AnimalType), out IList list))
        {
            for (int i = 0; i < count; i++)
            {
                list.Add(new AnimalType());
            }
        }
    }

    /*
      public int entityCount = 100;
    public float velocity = 0.1f;
    public GameObject prefab;

    private const int MAX_OBJS_PER_DRAWCALL = 1000;
    private Mesh prefabMesh;
    private Material prefabMaterial;
    private Vector3 prefabScale;

    private List<uint> entities;

    void Start()
    {
        ECSManager.Init();
        entities = new List<uint>();
        for (int i = 0; i < entityCount; i++)
        {
            uint entityID = ECSManager.CreateEntity();
            ECSManager.AddComponent<PositionComponent>(entityID,
                new PositionComponent(0, -i, 0));
            ECSManager.AddComponent<VelocityComponent>(entityID,
                new VelocityComponent(velocity, Vector3.right.x, Vector3.right.y, Vector3.right.z));
            entities.Add(entityID);
        }

        prefabMesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        prefabMaterial = prefab.GetComponent<MeshRenderer>().sharedMaterial;
        prefabScale = prefab.transform.localScale;
    }

    void Update()
    {
        ECSManager.Tick(Time.deltaTime);
    }

    void LateUpdate()
    {
        List<Matrix4x4[]> drawMatrix = new List<Matrix4x4[]>();
        int meshes = entities.Count;
        for (int i = 0; i < entities.Count; i += MAX_OBJS_PER_DRAWCALL)
        {
            drawMatrix.Add(new Matrix4x4[meshes > MAX_OBJS_PER_DRAWCALL ? MAX_OBJS_PER_DRAWCALL : meshes]);
            meshes -= MAX_OBJS_PER_DRAWCALL;
        }
        Parallel.For(0, entities.Count, i =>
        {
            PositionComponent position = ECSManager.GetComponent<PositionComponent>(entities[i]);
            drawMatrix[(i / MAX_OBJS_PER_DRAWCALL)][(i % MAX_OBJS_PER_DRAWCALL)]
            .SetTRS(new Vector3(position.X, position.Y, position.Z), Quaternion.identity, prefabScale);
        });
        for (int i = 0; i < drawMatrix.Count; i++)
        {
            Graphics.DrawMeshInstanced(prefabMesh, 0, prefabMaterial, drawMatrix[i]);
        }
    }
    */
}
