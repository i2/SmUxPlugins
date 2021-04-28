namespace Templates
{
    public class BlockInformation
    {
        public TemplateFieldInfo BeginBlockTag { get; set;}
        public TemplateFieldInfo EndBlockTag { get; set; }

        public BlockInformation(TemplateFieldInfo beginBlockTag, TemplateFieldInfo endBlockTag)
        {
            BeginBlockTag = beginBlockTag;
            EndBlockTag = endBlockTag;
        }
    }
}