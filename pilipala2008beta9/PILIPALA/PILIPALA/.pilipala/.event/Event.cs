using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PILIPALA.Event
{
    public class Event
    {
        public delegate void BeforeEvent();
        public event BeforeEvent Doc_head_before;
        public event BeforeEvent Doc_body_before;
        public event BeforeEvent Doc_foot_before;
        public event BeforeEvent Doc_all_before;
        public event BeforeEvent Post_load_before;
        public event BeforeEvent PostContent_get_before;

        public delegate void AfterEvent();
        public event AfterEvent Doc_head_after;
        public event AfterEvent Doc_body_after;
        public event AfterEvent Doc_foot_after;
        public event AfterEvent Doc_all_after;
        public event AfterEvent Post_load_after;
        public event AfterEvent PostContent_get_after;

    }
}
