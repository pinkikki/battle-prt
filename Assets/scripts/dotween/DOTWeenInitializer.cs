using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace dotween
{
    public class DOTWeenInitializer : MonoBehaviour, Initializer
    {
        public void Init()
        {
            DOTween.Init();
            DOTween.defaultAutoPlay = AutoPlay.None;
        }
    }
}