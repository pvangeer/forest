using Forest.Data;
using Forest.Data.Tree;

namespace Forest.Calculators
{
    public class CriticalPathElement
    {
        public CriticalPathElement(TreeEvent treeEvent, FragilityCurve fragilityCurve, bool failElement)
        {
            Element = treeEvent;
            FragilityCurve = fragilityCurve;
            ElementFails = failElement;
        }

        public TreeEvent Element { get; }

        public bool ElementFails { get; }

        public FragilityCurve FragilityCurve { get; }
    }
}