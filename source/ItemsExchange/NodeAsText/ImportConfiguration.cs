using System;
using System.Collections.Generic;

namespace ItemsExchange.NodeAsText
{
    public class ImportConfiguration
    {
        public virtual void AdjustComponent(Component component)
        {
        }

        public virtual bool IsLessonTitleComponent(Component component)
        {
            return false;
        }

        public virtual bool IsChapterTitleComponent(Component component)
        {
            return false;
        }

        public virtual bool IsQuestionTitleComponent(Component component)
        {
            return false;
        }

        public Dictionary<string, string> Translations { get; set; }
    }

    public class GrammarTestsImportConfiguration
    { 
        public virtual void AdjustComponent(Component component)
        {
            if (component.IsTextType && component.HasText)
            {
                component.Text = component.Text.Replace(
                    "Stanis³aw P. Kaczmarski \"Verb Forms in Bilingual Exercises and Tests\"",
                    "Stanis³aw P. Kaczmarski \"VF in BE&T\"");

                component.Text = component.Text.Replace(
                    "Stanis³aw P. Kaczmarski \"EXPERIENCE through PRACTICE\"",
                    "Stanis³aw P. Kaczmarski \"ETP\"");

                component.Text = component.Text.Replace(
                    "Verb Forms Multiple-Choice Test",
                    "Verb Forms Test");

                component.Text = component.Text.Replace(
                    "Lexical Items Multiple-Choice Test",
                    "Lexical Items Test");

                component.Text = component.Text.Replace("(a) ", string.Empty);
                component.Text = component.Text.Replace("(b) ", string.Empty);
                component.Text = component.Text.Replace("(c) ", string.Empty);
                component.Text = component.Text.Replace("(d) ", string.Empty);
                
                component.Text = component.Text.Replace("a. ", string.Empty);
                component.Text = component.Text.Replace("b. ", string.Empty);
                component.Text = component.Text.Replace("c. ", string.Empty);
                component.Text = component.Text.Replace("d. ", string.Empty);

                component.Text = component.Text.Replace("(a)", string.Empty);
                component.Text = component.Text.Replace("(b)", string.Empty);
                component.Text = component.Text.Replace("(c)", string.Empty);
                component.Text = component.Text.Replace("(d)", string.Empty);

                component.Text = component.Text.Replace("(", "<SPAN style=\"COLOR: #8080ff\">(");
                component.Text = component.Text.Replace(")", ")</SPAN>");
            }
        }

        public virtual bool IsLessonTitleComponent(Component component)
        {
            if (component.IsTextType && component.HasText && component.Text.StartsWith("Stanis³aw P. Kaczmarski \"VF in BE&T\""))
            {
                return true;
            }

            if (component.IsTextType && component.HasText && component.Text.StartsWith("Stanis³aw P. Kaczmarski \"ETP\""))
            {
                return true;
            }

            return false;
        }

        public virtual bool IsChapterTitleComponent(Component component)
        {
            if (component.IsTextType && component.HasText)
            {
                if (component.Text.StartsWith("Test "))
                {
                    return true;
                }

                if (component.Text.StartsWith("Final test"))
                {
                    return true;
                }

                if (component.Text.StartsWith("Strona "))
                {
                    return true;
                }

                if (component.Text.StartsWith("Multiple-Choice Test"))
                {
                    return true;
                }

                if (component.Text.StartsWith("Lexical Items Test"))
                {
                    return true;
                }

                if (component.Text.StartsWith("Verb Forms Test"))
                {
                    return true;
                }
            }


            return false;
        }

        public virtual bool IsQuestionTitleComponent(Component component)
        {
            if (component.IsTextType && component.HasText && component.Text.StartsWith("Wybierz prawid³ow¹ w jêzyku angielskim wersjê podanej w nawiasach polskiej"))
            {
                return true;
            }

            if (component.IsTextType && component.HasText && component.Text.StartsWith("Wybierz prawid³ow¹ w jêzyku angielskim wersjê podanego w nawiasach polskiego wyrazu lub zwrotu."))
            {
                return true;
            }

            return false;
        }
    }
}