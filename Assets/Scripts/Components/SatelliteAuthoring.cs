using System.IO;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public class SatelliteAuthoring : MonoBehaviour
    {
        class Baker : Baker<SatelliteAuthoring>
        {
            public override void Bake(SatelliteAuthoring authoring)
            {
                string tleAll = Resources.Load<TextAsset>("3le").text;
                string[] tleLines = tleAll.Split('\n');
                
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SatelliteComponent
                {
                    
                });
            }
        }
    }
    
    public struct SatelliteComponent : IComponentData
    {
        public FixedString128Bytes Tle1;
        public FixedString128Bytes Tle2;
    }

    public struct SatellitePositions : IComponentData
    {
        public NativeArray<float3> Positions;
    }

    public struct SatelliteInfo : IComponentData
    {
        //public char[] NoradID;
        public float Period;

    }
}