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
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            i = 0;
        }

        public void OnUpdate(ref SystemState state)
        {
            int time = TimeManager.Instance.GetTime();
            foreach (var (transform, sat) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SatellitePositions>>())
            {
                transform.ValueRW.Position = sat.ValueRO.Positions[time % sat.ValueRO.Positions.Length];
            }

            i++;
        }

        [BurstCompile]
        private float3 GetPosition(string line1, string line2)
        {
            TwoLineElements tle = new TwoLineElements("", line1, line2);
            Satellite satellite = new Satellite(tle);

            Eci eci = satellite.PositionEci(DateTime.Now);
            Vector3 position = new Vector3((float)eci.Position.X, (float)eci.Position.Y, (float)eci.Position.Z);
            return position / 1000;
        }
    }
}