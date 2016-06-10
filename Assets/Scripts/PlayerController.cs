using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private Texture2D crossMissTexture;
    [SerializeField]
    private Texture2D crossTexture;
    [SerializeField]
    private Vector2 crossOffset = new Vector2(16, 16);

    private BulletsEmiter bulletsEmiter;
    private TowerController towerController;
    private MovementController movementController;
    private Camera mainCamera;
    private TerrainCollider terrainCollider;

    RaycastHit lastCursorHitInfo;
    bool canShoot;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        terrainCollider = GetComponentInChildren<TerrainCollider>();
        bulletsEmiter = GetComponentInChildren<BulletsEmiter>();
        towerController = GetComponentInChildren<TowerController>();
        movementController = GetComponentInChildren<MovementController>();
        
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        Ray crossRay = mainCamera.ScreenPointToRay(Input.mousePosition);
       
        if (!terrainCollider.Raycast(crossRay, out lastCursorHitInfo, 10000))
        {
            canShoot = false;
        }
        else
        {
             float sqrRange = (towerController.transform.position - lastCursorHitInfo.point).sqrMagnitude;
            if (sqrRange > bulletsEmiter.MaxRange*bulletsEmiter.MaxRange
                || sqrRange < bulletsEmiter.MinRange*bulletsEmiter.MinRange)
            {
                canShoot = false;
            }
            else
            {
                float restAngle = towerController.LookAt(lastCursorHitInfo.point);
                if (Math.Abs(restAngle) < 0.001f)
                {
                    canShoot = true;
                }
                else
                {
                    canShoot = false;
                    movementController.Rotate(restAngle);
                }
            }
        }

        Cursor.SetCursor(canShoot ? crossTexture : crossMissTexture, crossOffset, CursorMode.Auto);


        if (canShoot && Input.GetMouseButtonUp(0))
        {
             bulletsEmiter.CmdFire(lastCursorHitInfo.point);
        }
    }


}
