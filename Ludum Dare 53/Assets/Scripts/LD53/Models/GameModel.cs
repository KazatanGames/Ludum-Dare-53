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
    public class GameModel : Singleton<GameModel>
    {
        public Vector3 dronePosition;
        public Vector3 droneVelocity;
        public Vector3 droneAcceleration;
        public Vector3 dronePlayerAccel;
        public Vector3 droneWindAccel;
        public float droneSpeed;
        public bool playerFiring;
        public bool playerFired;
        public float nextFire;
        public Vector2 aimInput;
        public float gameTime;
        public int gameScore;
        public float introTime;
        public float introRatio;

        public WorldData world;

        public event Action<Vector3, Vector3> OnFire;
        public event Action OnReset;
        public event Action OnGameOver;

        public void Reset()
        {
            dronePosition = new(
                ((LD53AppManager.INSTANCE.AppConfig.playAreaSize.x + 1f) * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f,
                0f,
                ((LD53AppManager.INSTANCE.AppConfig.playAreaSize.y + 1f) * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f
            );
            droneVelocity = Vector3.zero;
            droneAcceleration = Vector3.zero;
            dronePlayerAccel = Vector3.zero;
            droneWindAccel = Vector3.zero;
            droneSpeed = 0;
            playerFiring = false;
            aimInput = Vector2.zero;
            nextFire = 0f;
            world = WorldGen.Generate(LD53AppManager.INSTANCE.AppConfig.playAreaSize.x, LD53AppManager.INSTANCE.AppConfig.playAreaSize.y);
            gameTime = 0f;
            gameScore = 0;
            introTime = 0f;
            introRatio = 0f;

            OnReset?.Invoke();
        }

        public void Tick(float tickTime)
        {
            if (nextFire > 0) nextFire -= tickTime;
            if (playerFired)
            {
                if (!playerFiring) playerFired = false;
            }
            else
            {
                if (nextFire <= 0f && playerFiring)
                {
                    OnFire?.Invoke(dronePosition, droneVelocity + new Vector3(0f, LD53AppManager.INSTANCE.AppConfig.parcelYFireSpeed, 0f));
                    playerFired = true;
                    nextFire = LD53AppManager.INSTANCE.AppConfig.fireDelay;
                }
            }
            gameTime += tickTime;
        }

        public void Frame(float frameTime)
        {
            droneAcceleration = dronePlayerAccel + droneWindAccel;

            DroneMovement(frameTime);
        }

        public void IntroFrame(float frameTime)
        {
            introTime += frameTime;
            introRatio = introTime / LD53AppManager.INSTANCE.AppConfig.droneIntroLength;
            introRatio = Mathf.Clamp(introRatio, 0f, 1f);
            float droneHeight = Easing.Quadratic.InOut(introRatio) * LD53AppManager.INSTANCE.AppConfig.droneHeight;
            dronePosition = new(dronePosition.x, droneHeight, dronePosition.z);
        }

        public void TargetHit(GridPos pos)
        {
            if (LD53AppManager.INSTANCE.Common.chosenGameMode == GameMode.TargetHunt)
            {
                world.cells[pos.x, pos.z].targetHuntTarget = false;
                gameScore++;

                if (gameScore >= world.targetCount)
                {
                    OnGameOver?.Invoke();
                }
            } else
            {
                // TODO: time attack
            }
        }

        protected void DroneMovement(float frameTime)
        {
            Vector3 newDronePosition = dronePosition;
            Vector3 newDroneVelocity = droneVelocity;

            // accel
            newDroneVelocity += droneAcceleration * frameTime;

            // drag
            Vector3 drag = newDroneVelocity * LD53AppManager.INSTANCE.AppConfig.droneDragCoeff;
            newDroneVelocity -= drag * frameTime;

            // movement
            newDronePosition += newDroneVelocity * frameTime;

            // checks
            if (newDronePosition.x < 0)
            {
                newDronePosition.x = -newDronePosition.x;
                newDroneVelocity.x = -newDroneVelocity.x * LD53AppManager.INSTANCE.AppConfig.droneWallBound;
            }
            if (newDronePosition.z < 0)
            {
                newDronePosition.z = -newDronePosition.z;
                newDroneVelocity.z = -newDroneVelocity.z * LD53AppManager.INSTANCE.AppConfig.droneWallBound;
            }

            float maxX = LD53AppManager.INSTANCE.AppConfig.playAreaGridSize * LD53AppManager.INSTANCE.AppConfig.playAreaSize.x;
            float maxY = LD53AppManager.INSTANCE.AppConfig.playAreaGridSize * LD53AppManager.INSTANCE.AppConfig.playAreaSize.y;

            if (newDronePosition.x > maxX)
            {
                newDronePosition.x = maxX - (newDronePosition.x - maxX);
                newDroneVelocity.x = -newDroneVelocity.x * LD53AppManager.INSTANCE.AppConfig.droneWallBound;
            }
            if (newDronePosition.z > maxY)
            {
                newDronePosition.z = maxY - (newDronePosition.z - maxY);
                newDroneVelocity.z = -newDroneVelocity.z * LD53AppManager.INSTANCE.AppConfig.droneWallBound;
            }

            // calc new speed
            float newDroneSpeed = newDroneVelocity.magnitude;

            // clamp speed
            if (newDroneSpeed <= 0.001f)
            {
                newDroneVelocity = Vector3.zero;
                newDroneSpeed = 0f;
            }
            else if (newDroneSpeed > LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed)
            {
                newDroneVelocity = newDroneVelocity.normalized * LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed;
                newDroneSpeed = LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed;
            }

            dronePosition = newDronePosition;
            droneVelocity = newDroneVelocity;
            droneSpeed = newDroneSpeed;
        }
    }
}