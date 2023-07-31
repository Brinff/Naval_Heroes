using DG.Tweening;
using Game.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNotificationWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private float m_Duration = 3f;

    [SerializeField]
    private List<PlayerNotify> m_Prefabs = new List<PlayerNotify>();

    private PlayerNotify m_CurrentNotify;

    [SerializeField]
    private RectTransform m_Layout;

    //private LayoutGroup m_LayoutGroup;

    [SerializeField]
    private int m_CountVisible = 1;

    private List<PlayerNotify> m_Notifications = new List<PlayerNotify>();

    public void AddNotify<T>() where T : Component
    {
        var notify = m_Prefabs.Find(x => x is T);
        if (notify == null)
        {
            return;
        }

        var notifyInstance = Instantiate(notify);
        notifyInstance.gameObject.SetActive(true);
        notifyInstance.transform.SetParent(m_Layout);
        notifyInstance.Show(this);
        Rebuild();

        HideNotify(notifyInstance, m_Duration);

        m_Notifications.Add(notifyInstance);

        if (m_Notifications.Count > m_CountVisible)
        {
            HideNotify(m_Notifications[0], 0);
        }
    }

    private void HideNotify(PlayerNotify notify, float delay)
    {
        notify.Hide().SetDelay(delay).OnComplete(() => { Destroy(notify.gameObject,1); m_Notifications.Remove(notify); });  
    }

    [Button]
    public void AddNotify()
    {
        AddNotify<EnemyKillNotify>();
    }

    public void Rebuild()
    {
        LayoutRebuilder.MarkLayoutForRebuild(m_Layout);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }
}
