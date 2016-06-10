using Assets.Helpers;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class TankNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject playersContainer;
        [SerializeField] private GameObject playerCamera;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject player = (GameObject)Instantiate(playerPrefab);
            player.AppendTo(playersContainer);
            player.transform.localPosition = new Vector3(958, 0, 579);

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            NetworkBehaviour playerController = player.GetComponent<NetworkBehaviour>();
            if (playerController.isLocalPlayer)
            {
                playerCamera.AppendTo(player);
                playerCamera.transform.localPosition = Vector3.zero;
            }
        }
    }
}