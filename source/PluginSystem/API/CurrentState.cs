namespace PluginSystem.API
{
    public static class CurrentState
    {
        public static bool IsCourseLoaded
        {
            get
            {
                return !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath);
            }
        }

        public static bool IsVisualizerModeZero
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public static bool IsInEditMode
        {
            get 
            {
                return SuperMemo.GetItemsTreeView() != null && !string.IsNullOrEmpty(SuperMemo.CurrentCoursePath) && SuperMemo.VisualizerMode == 1;
            }
        }

        public static bool InLearningModeInQuestionPart
        {
            get
            {
                return SuperMemo.VisualizerMode == 2;
            }
        }

        public static bool InLearningModeInAnswerPart
        {
            get
            {
                return SuperMemo.VisualizerMode == 3;
            }
        }
    }
}