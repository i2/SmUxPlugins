using System;
using System.Collections.Generic;
using PluginSystem.Helpers;

namespace Templates
{
    public class TemplateResolvingData
    {
        private readonly IDictionary<string, string> m_Fields = new Dictionary<string, string>();
        private readonly IDictionary<string, string> m_FieldsToTags = new Dictionary<string, string>();

        private readonly IDictionary<string, BlockInformation> m_BlocksInfo = new Dictionary<string, BlockInformation>();
        private readonly List<string> m_BlocksToClear = new List<string>();

        public TemplateResolvingData(Template template, IDictionary<string, string> fields)
        {
            m_Fields = fields;

            foreach (KeyValuePair<string, string> pair in m_Fields)
            {
                var templateFieldInfo = new TemplateFieldInfo(pair.Key);
                m_FieldsToTags[templateFieldInfo.FieldName] = pair.Value;
            }

            var blocksBegin = new Dictionary<string, TemplateFieldInfo>();
            var blocksEnd = new Dictionary<string, TemplateFieldInfo>();

            foreach (TemplateFieldInfo templateFieldInfo in template.TagsInfo)
            {
                if (templateFieldInfo.IsBlock)
                {
                    if(templateFieldInfo.IsBlockBegin)
                    {
                        blocksBegin[templateFieldInfo.GetBlockBeginName()] = templateFieldInfo;
                    }
                    else if (templateFieldInfo.IsBlockEnd)
                    {
                        blocksEnd[templateFieldInfo.GetBlockEndName()] = templateFieldInfo;
                    }
                }
                else if (templateFieldInfo.IsField)
                {
                    bool hasEmptyValue = string.IsNullOrEmpty(m_FieldsToTags[templateFieldInfo.FieldName]);
                    
                    if (hasEmptyValue)
                    {
                        const string REMOVE_BLOCK_IF_EMPTY_LABEL = "IfEmptyRemoveBlock";

                        if (templateFieldInfo.Variables.ContainsKey(REMOVE_BLOCK_IF_EMPTY_LABEL))
                        {
                            m_BlocksToClear.Add((string) templateFieldInfo.Variables[REMOVE_BLOCK_IF_EMPTY_LABEL]);
                        }
                    }
                }
            }

            if (blocksBegin.Count!= blocksEnd.Count)
            {
                throw new ArgumentException("BeginBlock count differs from EndBlock count");
            }

            foreach (string blockName in blocksBegin.Keys)
            {
                m_BlocksInfo[blockName] = new BlockInformation(blocksBegin[blockName], blocksEnd[blockName]);
            }
        }

        public IDictionary<string, string> Fields
        {
            get { return m_Fields; }
        }

        public string RemoveAllBlocks(string text)
        {
            foreach (string blockToClear in m_BlocksToClear)
            {
                int startIndex = text.IndexOf(m_BlocksInfo[blockToClear].BeginBlockTag.OriginalTag);
                int endIndex = text.IndexOf(m_BlocksInfo[blockToClear].EndBlockTag.OriginalTag);

                if (startIndex != -1 && endIndex != -1)
                {
                    if (endIndex > startIndex)
                    {
                        text = text.Remove(startIndex, endIndex - startIndex);
                    }
                    else
                    {
                        throw new InvalidOperationException("EndBlock before BeginBlock");
                    }
                }
            }

            foreach (KeyValuePair<string, BlockInformation> info in m_BlocksInfo)
            {
                text = StringHelper.RemoveFromText(text, info.Value.BeginBlockTag.OriginalTag);
                text = StringHelper.RemoveFromText(text, info.Value.EndBlockTag.OriginalTag);
            }

            return text;
        }
    }
}