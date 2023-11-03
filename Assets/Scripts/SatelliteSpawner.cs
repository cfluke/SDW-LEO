using System;
using Components;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Zeptomoby.OrbitTools;

public class SatelliteSpawner : MonoBehaviour
{
    [SerializeField] private TextAsset tleTextAsset; // Assign your text file in the Unity inspector.
    [SerializeField] private GameObject satellitePrefab;

    private void Start()
    {
        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        string tleText = Resources.Load<TextAsset>("full_catalog").text;
        string[] tleLines = tleText.Split('\n');
        
        Mesh cubeMesh = satellitePrefab.GetComponent<MeshFilter>().sharedMesh;
        Material cubeMat = satellitePrefab.GetComponent<MeshRenderer>().sharedMaterial;

        for (int i = 0; i < tleLines.Length; i += 3)
        {
            Entity entity = em.CreateEntity();

            RenderMeshDescription desc = new RenderMeshDescription
            {
                FilterSettings = new RenderFilterSettings
                {
                    RenderingLayerMask = 1
                }
            };
            RenderMeshArray test = new RenderMeshArray(new[] { cubeMat }, new[] { cubeMesh });
            RenderMeshUtility.AddComponents(entity, em, desc, test, MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
            
            em.AddComponent<LocalTransform>(entity);
            em.SetComponentData(entity, new LocalTransform
            {
                Scale = 0.05f
            });
            em.AddComponentData(entity, new SatelliteComponent
            {
                Tle1 = tleLines[i + 1],
                Tle2 = tleLines[i + 2]
            });
            
            TwoLineElements tle = new TwoLineElements("", tleLines[i + 1], tleLines[i + 2]);
            Satellite satellite = new Satellite(tle);
            em.AddComponentData(entity, new SatellitePositions
            {
                Positions = PrecalculatePositions(satellite, 1)
            });
        }
    }

    private NativeArray<float3> PrecalculatePositions(Satellite satellite, int count)
    {
        float3[] output = new float3[count];

        for (int i = 0; i < count; i++)
        {
            try
            {
                DateTime start = DateTime.Now - TimeSpan.FromDays(30);
                Eci eci = satellite.PositionEci(start + TimeSpan.FromSeconds(1f) * i);
                Vector3 position = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y);
                output[i] = position / 1000;
            }
            catch (PropagationException e)
            {
                output[i] = new float3();
            }
        }

        return new NativeArray<float3>(output, Allocator.Persistent);
    }
}