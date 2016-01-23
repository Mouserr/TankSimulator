using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Texture2D crossMissTexture;
    [SerializeField]
    private Texture2D crossTexture;
    [SerializeField]
    private Vector2 crossOffset = new Vector2(16, 16);

    private BulletsEmiter bulletsEmiter;
    private Camera mainCamera;
    private TerrainCollider terrainCollider;

    RaycastHit lastCursorHitInfo;
    bool canShoot;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        terrainCollider = GetComponentInChildren<TerrainCollider>();
        bulletsEmiter = GetComponentInChildren<BulletsEmiter>();
        
    }

    private void Update()
    {
        Ray crossRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!terrainCollider.Raycast(crossRay, out lastCursorHitInfo, 10000)
            || (bulletsEmiter.transform.position - lastCursorHitInfo.point).sqrMagnitude > bulletsEmiter.MaxRange * bulletsEmiter.MaxRange)
        {
            Cursor.SetCursor(crossMissTexture, crossOffset, CursorMode.Auto);
            canShoot = false;
        }
        else
        {
            Cursor.SetCursor(crossTexture, crossOffset, CursorMode.Auto);
            canShoot = true;
        }

        if (canShoot && Input.GetMouseButtonUp(0))
        {
             bulletsEmiter.Fire(lastCursorHitInfo.point);
        }
    }


}
