using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public int entity;

    public int entityPlayer1;
    public int entityPlayer2;

    private void Start()
    {
        entityPlayer1 = EntityManager.Instance.world.Bake(player1);
        if(player2) entityPlayer2 = EntityManager.Instance.world.Bake(player2);

        this.entity = entityPlayer1;
        EntityManager.Instance.world.GetPool<PlayerTagLeo>().Add(entity);
    }

    [Button]
    public void Switch()
    {
        if(this.entity == entityPlayer1)
        {
            EntityManager.Instance.world.GetPool<PlayerTagLeo>().Del(entityPlayer1);
            EntityManager.Instance.world.GetPool<PlayerTagLeo>().Add(entityPlayer2);
            this.entity = entityPlayer2;
            return;
        }

        if (this.entity == entityPlayer2)
        {
            EntityManager.Instance.world.GetPool<PlayerTagLeo>().Del(entityPlayer2);
            EntityManager.Instance.world.GetPool<PlayerTagLeo>().Add(entityPlayer1);
            this.entity = entityPlayer1;
            return;
        }
    }
}
