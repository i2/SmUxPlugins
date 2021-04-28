using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ItemsExchange.NodeAsText
{
    public class QuestionAnswerRecord
    {
        readonly Dictionary<char, string> m_Lines = new Dictionary<char, string>();

        public bool IsEmpty
        {
            get
            {
                return m_Lines.Keys.Count > 0;
            }
        }

        public void SetLine(char type, string line)
        {
            m_Lines[type] = line;
        }

        public string GetLine(char type)
        {
            return m_Lines[type];
        }

        public static List<QuestionAnswerRecord> LoadQuestionAnswerFile(string fileName)
        {
            var result = new List<QuestionAnswerRecord>();

            using (var reader = new StreamReader(fileName, Encoding.Default, true))
            {
                var currentRecord = new QuestionAnswerRecord();

                while (!reader.EndOfStream)
                {
                    string currentLine = reader.ReadLine();
                    currentLine = currentLine.Trim();

                    if (currentLine.Length == 0)
                    {
                        result.Add(currentRecord);
                        currentRecord = new QuestionAnswerRecord();
                    }
                    else
                    {
                        if (char.IsLetter(currentLine, 0) && (currentLine[1] == ':') && (currentLine[2] == ' '))
                        {
                            currentRecord.SetLine(currentLine[0], currentLine.Substring(3));
                        }
                    }
                }

                if (!currentRecord.IsEmpty)
                {
                    result.Add(currentRecord);
                }
            }

            return result;
        }
    }
}