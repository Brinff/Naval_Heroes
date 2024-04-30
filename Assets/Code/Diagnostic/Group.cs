using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Diagnostic
{
    public class Group : VisualElement
    {
        private const string s_ClassName = "group-root";
        private const string s_HeadClassName = "group-head";
        private const string s_BodyClassName = "group-body";

        public new class UxmlFactory : UxmlFactory<Group, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_String =
                new UxmlStringAttributeDescription { name = "title", defaultValue = "default_value" };

            UxmlEnumAttributeDescription<FlexDirection> m_Direction =
                new UxmlEnumAttributeDescription<FlexDirection>
                    { name = "direction", defaultValue = FlexDirection.Column };


            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as Group;

                ate.title = m_String.GetValueFromBag(bag, cc);
                ate.direction = m_Direction.GetValueFromBag(bag, cc);
            }
        }


        private Label m_TitleLabel;
        private VisualElement m_Head;
        private VisualElement m_Body;

        public override VisualElement contentContainer => m_Body;

        public Group()
        {
            styleSheets.Add(Resources.Load<StyleSheet>($"Styles/GroupStyle"));
            AddToClassList(s_ClassName);
            m_Head = new VisualElement();
            m_Head.AddToClassList(s_HeadClassName);
            m_TitleLabel = new Label(m_Title);
            m_Head.Add(m_TitleLabel);
            hierarchy.Add(m_Head);

            m_Body = new VisualElement();
            m_Body.AddToClassList(s_BodyClassName);
            m_Body.style.flexDirection = new StyleEnum<FlexDirection>(m_Direction);
            hierarchy.Add(m_Body);
        }

        public Group(string title) : this()
        {
            this.title = title;
        }

        public Group(string title, FlexDirection direction) : this()
        {
            this.title = title;
            this.direction = direction;
        }


        private string m_Title = "Title";

        public string title
        {
            get { return m_Title; }
            set
            {
                m_Title = value;
                if (m_TitleLabel != null) m_TitleLabel.text = value;
            }
        }

        private FlexDirection m_Direction;

        public FlexDirection direction
        {
            get { return m_Direction; }
            set
            {
                m_Direction = value;
                if (m_Body != null) m_Body.style.flexDirection = new StyleEnum<FlexDirection>(value);
            }
        }
    }
}