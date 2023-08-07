using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerTracker : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private PointRenderer pointRenderer;

        private void Update()
        {
            pointRenderer.SetRefPosition(player.position);
        }
    }
}