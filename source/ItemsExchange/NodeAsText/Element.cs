using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ItemsExchange.NodeAsText
{
    public class Element
    {
        internal const string ELEMENT_BEGIN_LABEL = "Begin Element #";

        internal const string ELEMENT_END_LABEL = "End Element #";

        private const string PARENT_ID_LABEL = "Parent=";
        private const string TITLE_LABEL = "Title=";

        private List<Component> m_Components = new List<Component>();

        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Title { get; set ; }

        public bool HasMultiChoice
        {
            get
            {
                return Components.Exists(component => component.TestElement == 2);
            }
        }

        public List<Component> Components
        {
            get { return m_Components; }
            set { m_Components = value; }
        }

        public IEnumerable<Component> QuestionComponents
        {
            get
            {
                foreach (Component questionComponent in Components.Where(component => component.IsQuestionComponent))
                {
                    yield return questionComponent;
                }

                yield break;
            }
        }

        public IEnumerable<Component> AnswerComponents
        {
            get
            {
                foreach (Component answerComponent in Components.Where(component => component.IsAnswerComponent))
                {
                    yield return answerComponent;
                }

                yield break;
            }
        }

        public IEnumerable<Component> SoundComponents
        {
            get
            {
                foreach (Component answerComponent in Components.Where(component => component.IsSound))
                {
                    yield return answerComponent;
                }

                yield break;
            }
        }

        public IEnumerable<Component> MultiChoiceComponents
        {
            get 
            {
                foreach (Component multiChoiceComponent in Components.Where(component => component.TestElement > 0))
                {
                    yield return multiChoiceComponent;
                }

                yield break;
            }
        }

        private Element(int id)
        {
            Id = id;
        }

        public static Element Deserialize(int id, StreamReader streamReader)
        {
            var element = new Element(id);

            string line;
            do
            {
                line = streamReader.ReadLine().Trim();

                if (line.StartsWith(PARENT_ID_LABEL))
                {
                    element.ParentId = int.Parse(line.Substring(PARENT_ID_LABEL.Length));
                }
                else if (line.StartsWith(TITLE_LABEL))
                {
                    element.Title = line.Substring(TITLE_LABEL.Length);
                }
                else if (line.StartsWith(Component.BEGIN_LABEL))
                {
                    int componentId = int.Parse(line.Substring(Component.BEGIN_LABEL.Length));
                    Component component = Component.Deserialize(componentId, streamReader);
                    component.Element = element;
                    element.Components.Add(component);
                }
                
            } while (!streamReader.EndOfStream && !line.Equals(ELEMENT_END_LABEL + id));

            element.Components.Sort(ImportAsText.ComponentComparison);
            element.RemoveIrrelevantComponents();
            return element;
        }

        private void RemoveIrrelevantComponents()
        {
            Components.RemoveAll(component => component.IsSound && string.IsNullOrEmpty(component.SoundFileName));
        }
    }
}