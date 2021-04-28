using System.Windows.Forms;
using System.Xml;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace ItemsExchange
{
    public class ReverseQuestionAnswer : SuperMemoExtension
    {
        public override string Name
        {
            get { return @"&Jednostki\&Zamieñ pytanie z odpowiedzi¹"; }
        }

        public override bool Enabled
        {
            get
            {
                return !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.F2 | Keys.Control;
        }

        public override void Execute()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(SuperMemo.CurrentItemFullPathFileName);

            XmlNodeList questionElement = xmlDocument.GetElementsByTagName("question");
            XmlNodeList answerElement = xmlDocument.GetElementsByTagName("answer");
            TreeView view = SuperMemo.GetItemsTreeView();

            if (questionElement.Count > 0 && answerElement.Count > 0 && view.SelectedNode != null)
            {
                string question = questionElement[0].InnerXml;
                string answer = answerElement[0].InnerXml;
                
                questionElement[0].InnerXml = answer;
                answerElement[0].InnerXml = question;

                xmlDocument.Save(SuperMemo.CurrentItemFullPathFileName);

                var curentItemNumber = SuperMemo.GetCurentItemNumber();

                TreeNode otherNode = view.SelectedNode.NextNode ?? view.SelectedNode.PrevNode;

                if (otherNode != null)
                {//refresh
                    
                    var otherNodeNumber = (int) otherNode.Tag;
                    SuperMemo.GoToPage(otherNodeNumber);
                    SuperMemo.GoToPage(curentItemNumber);
                }
            }
        }
    }
}