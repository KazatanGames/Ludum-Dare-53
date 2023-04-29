using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class GameSceneManager : SingletonMonoBehaviour<GameSceneManager>
    {
        [Header("Settings")]
        [SerializeField]
        [Min(1)]
        protected int simulationFps = 60;

        // vars
        protected float timeBank;
        protected float frameTime;
        protected bool pausedForStart;
        protected int tick;
        protected GameControlsInputActions controls;

        // controllers
        protected DroneController droneController;

        protected override void Initialise()
        {
            base.Initialise();

            controls = new();

            Reset();
            Build();
        }

        private void Start()
        {
            controls.DroneFlying.Enable();
        }

        private void Update()
        {
            if (pausedForStart) return;

            DoInput();

            timeBank += Time.deltaTime;

            while(timeBank >= frameTime)
            {
                Tick();
                timeBank -= frameTime;
            }
        }

        protected void Reset()
        {
            timeBank = 0f;
            frameTime = 1f / simulationFps;
            pausedForStart = false;
            tick = 0;
        }

        protected void Build()
        {
            droneController = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.dronePrefab, transform).GetComponent<DroneController>();
        }

        protected void DoInput()
        {
            Vector2 moveInput = controls.DroneFlying.Movement.ReadValue<Vector2>();

            float heightAccel = 0f;

            GameModel.Current.dronePlayerAccel = new(LD53AppManager.INSTANCE.AppConfig.droneAcceleration * moveInput.x, heightAccel, LD53AppManager.INSTANCE.AppConfig.droneAcceleration * moveInput.y);
        }

        protected void Tick()
        {
            tick++;
            GameModel.Current.Tick(frameTime);
        }

        public bool IsPaused => pausedForStart;
    }
}