using UnityEngine;

namespace battle.action
{
    public class ParticleMonoBehaviour : MonoBehaviour
    {
        void OnParticleSystemStopped()
        {
            Debug.Log("ParticleSystem!!!");
            BattleSystem.Instance.Subject.OnNext(BattleSystem.Status.AfterAction);
            Destroy(gameObject);
        }
    }
}
