using System.Collections.Generic;

namespace StoryTree.IO.Import.DotFormValidation
{
    public class DotFormValidationResult
    {
        public ExpertValidationResult ExpertValidation { get; set; }
        public Dictionary<DotNode, NodeValidationResult> NodesValidationResult { get; set; }
    }
}