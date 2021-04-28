using System;

namespace EditorExtension.AutoFormat
{
    [Flags]
    public enum ItemTextElements
    {
        Answer = 1, 
        ChapterTitle = 2, 
        LessonTitle = 4, 
        Question = 8, 
        QuestionTitle = 16,
        All = Answer | ChapterTitle | LessonTitle | Question | QuestionTitle,
    }
}