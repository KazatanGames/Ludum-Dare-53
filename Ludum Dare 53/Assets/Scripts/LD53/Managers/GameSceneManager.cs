using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
        [Header("In Scene")]
        [SerializeField]
        protected CinemachineVirtualCameraBase topDownCamera;
        [SerializeField]
        protected Transform backingBlackQuad;

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
        protected Transform container;
        protected Plane p = new(Vector3.left, Vector3.forward, Vector3.right);

        // controllers
        protected DroneController droneController;
        protected List<OfficeBuildingController> officeBuildingControllers;

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
            GameModel.Current.OnFire += OnPlayerFire;
            controls.DroneFlying.Fire.performed += OnPlayerPowerUpStart;
            controls.DroneFlying.Fire.canceled += OnPlayerPowerUpEnd;
        }

        private void OnDestroy()
        {
            GameModel.Current.OnFire -= OnPlayerFire;
            if (controls != null)
            {
                controls.DroneFlying.Fire.performed -= OnPlayerPowerUpStart;
                controls.DroneFlying.Fire.canceled -= OnPlayerPowerUpEnd;
            }
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

            GameModel.Current.Frame(Time.deltaTime);
        }

        protected void Reset()
        {
            officeBuildingControllers = new();
            timeBank = 0f;
            frameTime = 1f / simulationFps;
            pausedForStart = false;
            tick = 0;

            if (container != null) Destroy(container.gameObject);

            GameModel.Current.Reset();
        }

        protected void Build()
        {
            if (droneController == null)
            {
                droneController = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.dronePrefab, transform).GetComponent<DroneController>();
                topDownCamera.Follow = droneController.transform;
                topDownCamera.LookAt = droneController.transform;
            }

            container = new GameObject("Game World Container").transform;
            container.parent = transform;

            for (int x = 0; x < LD53AppManager.INSTANCE.AppConfig.playAreaSize.x; x++)
            {
                for (int z = 0; z < LD53AppManager.INSTANCE.AppConfig.playAreaSize.y; z++)
                {
                    CellData cell = GameModel.Current.cells[x, z];
                    switch (cell.cellType)
                    {
                        case CellTypeEnum.Office:
                            BuildComponentCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuilding, x, z);
                            break;
                        case CellTypeEnum.Concrete:
                            BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.concreteGround, x, z);
                            break;
                        case CellTypeEnum.Grass:
                            BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.grassGround, x, z);
                            break;
                        case CellTypeEnum.Road:
                            GameDirection otherRoads = GameDirectionHelper.Nil;
                            if (x > 0 && GameModel.Current.cells[x - 1, z].cellType == CellTypeEnum.Road) otherRoads |= GameDirection.West;
                            if (z > 0 && GameModel.Current.cells[x, z - 1].cellType == CellTypeEnum.Road) otherRoads |= GameDirection.South;
                            if (x < LD53AppManager.INSTANCE.AppConfig.playAreaSize.x - 1 && GameModel.Current.cells[x + 1, z].cellType == CellTypeEnum.Road) otherRoads |= GameDirection.East;
                            if (z < LD53AppManager.INSTANCE.AppConfig.playAreaSize.y - 1 && GameModel.Current.cells[x, z + 1].cellType == CellTypeEnum.Road) otherRoads |= GameDirection.North;

                            if (otherRoads == GameDirectionHelper.All)
                            {
                                // x junc
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.roadXJunc, x, z);
                            } else if (otherRoads.Count() == 3)
                            {
                                // t junc
                                float rotation = 0f;
                                if (!otherRoads.Test(GameDirection.South))
                                {
                                    rotation = 270f;
                                } else if (!otherRoads.Test(GameDirection.East))
                                {
                                    rotation = 180f;
                                } else if (!otherRoads.Test(GameDirection.North))
                                {
                                    rotation = 90f;
                                }
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.roadTJunc, x, z, rotation);
                            } else if (otherRoads.Test(GameDirection.East | GameDirection.West) || otherRoads.Test(GameDirection.North | GameDirection.South))
                            {
                                // straight
                                float rotation = 0f;
                                if (!otherRoads.Test(GameDirection.North))
                                {
                                    rotation = 90f;
                                }
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.roadStraight, x, z, rotation);
                            } else if (otherRoads.Count() == 2)
                            {
                                // corner
                                float rotation = 0f;
                                if (otherRoads.Test(GameDirection.West | GameDirection.North))
                                {
                                    rotation = 270f;
                                }
                                else if (otherRoads.Test(GameDirection.South | GameDirection.West))
                                {
                                    rotation = 180f;
                                }
                                else if (otherRoads.Test(GameDirection.East | GameDirection.South))
                                {
                                    rotation = 90f;
                                }
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.roadCorner, x, z, rotation);
                            } else if (otherRoads.Count() == 1)
                            {
                                // deadend
                                float rotation = 0f;
                                if (otherRoads.Test(GameDirection.East))
                                {
                                    rotation = 270f;
                                }
                                else if (otherRoads.Test(GameDirection.North))
                                {
                                    rotation = 180f;
                                }
                                else if (otherRoads.Test(GameDirection.West))
                                {
                                    rotation = 90f;
                                }
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.roadDeadEnd, x, z, rotation);
                            } else
                            {
                                // fallback to concrete
                                BuildSimpleCell(LD53AppManager.INSTANCE.AppConfig.prefabRegister.concreteGround, x, z);
                            }
                            break;
                    }
                }
            }

            backingBlackQuad.transform.localScale = new(
                (LD53AppManager.INSTANCE.AppConfig.playAreaSize.x + 20f) * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize,
                (LD53AppManager.INSTANCE.AppConfig.playAreaSize.x + 20f) * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize,
                1f
            );
            backingBlackQuad.transform.localPosition = new(
                LD53AppManager.INSTANCE.AppConfig.playAreaSize.x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize / 2f,
                -0.01f,
                LD53AppManager.INSTANCE.AppConfig.playAreaSize.y * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize / 2f
            );
        }

        protected T BuildComponentCell<T>(T prefab, int x, int z) where T : MonoBehaviour
        {
            T cell = Instantiate(prefab, container).GetComponent<T>();
            cell.transform.localPosition = PositionHelpers.CenterOfGridPosInWorld(x, z);
            return cell;
        }

        protected Transform BuildSimpleCell(Transform prefab, int x, int z, float rotationY = 0f)
        {
            Transform cell = Instantiate(prefab, container);
            cell.localPosition = PositionHelpers.CenterOfGridPosInWorld(x, z);
            cell.Rotate(Vector3.up, rotationY);
            return cell;
        }

        protected void DoInput()
        {
            Vector2 moveInput = controls.DroneFlying.Movement.ReadValue<Vector2>();
            Vector2 aimInputRaw = controls.DroneFlying.Aiming.ReadValue<Vector2>();

            float heightAccel = 0f;

            GameModel.Current.dronePlayerAccel = new(LD53AppManager.INSTANCE.AppConfig.droneAcceleration * moveInput.x, heightAccel, LD53AppManager.INSTANCE.AppConfig.droneAcceleration * moveInput.y);

            Ray r = Camera.main.ScreenPointToRay(aimInputRaw);
            Vector2 aimInput = Vector2.zero;

            if (p.Raycast(r, out float distance))
            {
                Vector3 aimPoint = r.GetPoint(distance);
                aimInput = new Vector2(aimPoint.x, aimPoint.z);
            }

            GameModel.Current.aimInput = aimInput;
        }

        protected void Tick()
        {
            tick++;
            GameModel.Current.Tick(frameTime);
        }

        public bool IsPaused => pausedForStart;

        public void RestartClicked()
        {
            Reset();
            Build();
        }

        protected void OnPlayerPowerUpStart(InputAction.CallbackContext context)
        {
            GameModel.Current.playerFiring = true;
        }

        protected void OnPlayerPowerUpEnd(InputAction.CallbackContext context)
        {
            GameModel.Current.playerFiring = false;
        }

        protected void OnPlayerFire(Vector3 position, Vector3 velocity)
        {
            ParcelController pc = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.parcelPrefab, container).GetComponent<ParcelController>();
            pc.transform.localPosition = position - new Vector3(0f, LD53AppManager.INSTANCE.AppConfig.parcelDropOffset, 0f);
            pc.body.AddForce(velocity, ForceMode.VelocityChange);
        }
    }
}