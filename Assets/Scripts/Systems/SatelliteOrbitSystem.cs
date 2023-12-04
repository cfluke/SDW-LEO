using System;
using System.Collections.Generic;
using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Zeptomoby.OrbitTools;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SatelliteOrbitSystem : ISystem
    {
        private int i;
        private float _lerpFactor;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            i = 0;
            _lerpFactor = 0f;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            /*float time = Time.deltaTime;
            
            foreach (var (transform, sat) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SatellitePositions>>())
            {
                //var job = new CalculatePositionJob();
                //job.Schedule();
                transform.ValueRW.Position = sat.ValueRO.Positions[i % sat.ValueRO.Positions.Length];
            }

            i++;*/
            
            foreach (var (transform, sat) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SatellitePositions>>())
            {
                int currentIndex = Mathf.FloorToInt(_lerpFactor) % sat.ValueRO.Positions.Length;
                int nextIndex = (currentIndex + 1) % sat.ValueRO.Positions.Length;

                Vector3 currentPosition = sat.ValueRO.Positions[currentIndex];
                Vector3 nextPosition = sat.ValueRO.Positions[nextIndex];

                transform.ValueRW.Position = Vector3.Lerp(currentPosition, nextPosition, _lerpFactor % 1f);
            }

            // Increment the lerp factor for the next frame
            _lerpFactor += 0.1f * Time.deltaTime * 20.0f; // You can adjust the speed of the lerp here
        }
    }

    // [BurstCompile]
    // partial struct CalculatePositionJob : IJobEntity
    // {
    //     void Execute(ref LocalTransform transform, in SatelliteComponent satellite)
    //     {
    //         transform.Position = GetPosition(satellite.Tle1.ToString(), satellite.Tle2.ToString());
    //     }
    //     
    //     private float3 GetPosition(string line1, string line2)
    //     {
    //         TwoLineElements tle = new TwoLineElements("", line1, line2);
    //         Satellite satellite = new Satellite(tle);
    //
    //         Eci eci = satellite.PositionEci(DateTime.Now);
    //         Vector3 position = new Vector3((float)eci.Position.X, (float)eci.Position.Y, (float)eci.Position.Z);
    //         return position / 1000;
    //     }
    // }
}