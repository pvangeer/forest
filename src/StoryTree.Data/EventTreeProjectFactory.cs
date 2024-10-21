using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryTree.Data
{
    public static class EventTreeProjectFactory
    {
        public static EventTreeProject CreateEmptyProject()
        {
            return new EventTreeProject();
        }

        public static EventTreeProject CreateStandardNewProject()
        {
            return new EventTreeProject();
        }
    }
}
