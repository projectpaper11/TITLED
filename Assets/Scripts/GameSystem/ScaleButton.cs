using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem
{
    public class ScaleButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        }
    }
}