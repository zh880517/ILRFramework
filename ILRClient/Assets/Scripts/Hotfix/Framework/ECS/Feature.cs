using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Core
{
    public class Feature
    {
        protected List<IInitializeSystem> initializes = new List<IInitializeSystem>();
        protected List<IExecuteSystem> executes = new List<IExecuteSystem>();
        protected List<ICleanupSystem> cleanups = new List<ICleanupSystem>();
        protected List<ITearDownSystem> tearDowns = new List<ITearDownSystem>();

        public void AddSystem()
        {

        }

    }


}