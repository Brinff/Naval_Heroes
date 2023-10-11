using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroup<Update>
{
    private Queue<ITutorial> m_ToDoTutorials = new Queue<ITutorial>();
    private List<ITutorial> m_ProcessTutorials = new List<ITutorial>();
    private List<ITutorial> m_DoneTutorials = new List<ITutorial>();
    private EcsWorld m_World;
    private IEcsSystems m_Systems;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Systems = systems;
    }
    

    public void HomeTutorial()
    {
        m_ToDoTutorials = new Queue<ITutorial>(transform.GetComponentsInChildren<ITutorial>());
        foreach (var tutorial in m_ToDoTutorials)
        {
            tutorial.Prepare(m_World, m_Systems);
        }
    }

    private ITutorial m_ProccesTutorial;
    public void Run(IEcsSystems systems)
    {
        if (m_ProccesTutorial == null)
        {
            if (m_ToDoTutorials.TryPeek(out ITutorial tutorial))
            {
                if (tutorial.IsDone())
                {
                    m_ToDoTutorials.Dequeue();
                }
                else
                {
                    if (tutorial.ConditionLaunch(m_World, systems))
                    {
                        m_ProccesTutorial = m_ToDoTutorials.Dequeue();
                        m_ProccesTutorial.Launch(m_World, systems);
                    }
                }
            }
        }
        else
        {
            m_ProccesTutorial.Process(m_World, systems);

            if (m_ProccesTutorial.ConditionDone(m_World, systems))
            {           
                m_ProccesTutorial.Done(m_World, systems);
                m_ProccesTutorial = null;
            }
        }

        //for (int i = 0; i < m_ProcessTutorials.Count; i++)
        //{
        //    var tutorial = m_ProcessTutorials[i];
        //    tutorial.Process(m_World, systems);
        //    if (tutorial.ConditionDone(m_World, systems))
        //    {
        //        if (m_ProcessTutorials.Remove(tutorial))
        //        {
        //            tutorial.Done(m_World, systems);
        //            m_DoneTutorials.Add(tutorial);
        //        }
        //    }
        //}
    }
}
