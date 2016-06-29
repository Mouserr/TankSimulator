using Assets.Helpers;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class TankNetworkManager : NetworkManager
    {
        [SerializeField] private GameObject playersContainer;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject player = (GameObject)Instantiate(playerPrefab);
			
		    player.AppendTo(playersContainer);
			player.transform.position = startPositions[0].position;
			startPositions.RemoveAt(0);
			
	        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
			PlayerController controller = player.GetComponent<PlayerController>();
            
			controller.Init();
        }
    }
}