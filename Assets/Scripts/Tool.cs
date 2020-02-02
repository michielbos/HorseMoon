using UnityEngine;

namespace HorseMoon
{

    public abstract class Tool : MonoBehaviour
    {
        // Whatever is spawned when the player equips the tool.
        public GameObject toolPrefab;
        public float cooldown = .5f;

        public AudioClip audioClip;
        public float audioVolume;

        public void PlayAudio()
        {
            if (audioClip)
            {
                AudioPool.PlaySound(transform.position, audioClip);
            }
        }


        public virtual void OnEquipped() { }

        public virtual void OnUnequipped() { }

        public virtual bool CanUse(Player player, InteractionObject target)
        {
            return false;
        }

        public virtual bool CanUse(Player player, Vector2Int target)
        {
            return false;
        }

        public virtual void UseTool(Player player, InteractionObject target, GameObject toolObject)
        {

        }

        public virtual void UseTool(Player player, Vector2Int target, GameObject toolObject)
        {

        }
    }

}