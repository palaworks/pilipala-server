using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PILIPALA.Event
{
    public class Event
    {
        public delegate void BeforeEvent();
        public static void CoreReadyBeforeEventHandler()
        {
            CoreReadyBefore += () => { };
            CoreReadyBefore();
        }
        public static event BeforeEvent CoreReadyBefore;

        public event BeforeEvent DocHeadBefore;
        public event BeforeEvent Doc_body_before;
        public event BeforeEvent Doc_foot_before;
        public event BeforeEvent Doc_all_before;

        public event BeforeEvent Post_load_before;
        public event BeforeEvent Post_create_before;
        public event BeforeEvent Post_delete_before;
        public event BeforeEvent PostContent_get_before;

        public event BeforeEvent Comment_create_before;
        public event BeforeEvent Comment_delete_delete;



        public delegate void AfterEvent();
        public static void CoreReadyAfterEventHandler()
        {
            CoreReadyAfter += () => { };
            CoreReadyAfter();
        }
        public static event AfterEvent CoreReadyAfter;

        public event AfterEvent Doc_head_after;
        public event AfterEvent Doc_body_after;
        public event AfterEvent Doc_foot_after;
        public event AfterEvent Doc_all_after;

        public event AfterEvent Post_load_after;
        public event AfterEvent Post_create_after;
        public event AfterEvent Post_delete_after;
        public event AfterEvent PostContent_get_after;

        public event AfterEvent Comment_create_after;
        public event AfterEvent Comment_delete_after;
    }
}
