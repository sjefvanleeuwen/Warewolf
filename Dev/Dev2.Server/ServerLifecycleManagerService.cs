﻿using Dev2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Dev2
{
    public class ServerLifecycleManagerService : ServiceBase, IDisposable
    {
        private readonly IServerLifecycleManager _serverLifecycleManager;
        public bool RunSuccessful { get; private set; }

        public ServerLifecycleManagerService()
            :this(new ServerLifecycleManager())
        {
        }
        public ServerLifecycleManagerService(IServerLifecycleManager serverLifecycleManager)
        {
            serverLifecycleManager.InteractiveMode = false;
            _serverLifecycleManager = serverLifecycleManager;
            CanPauseAndContinue = false;
        }

        protected override void OnStart(string[] args)
        {
            Dev2Logger.Info("** Service Started **", GlobalConstants.WarewolfInfo);
            RunSuccessful = true;
            _serverLifecycleManager.Run();
        }

        protected override void OnStop()
        {
            Dev2Logger.Info("** Service Stopped **", GlobalConstants.WarewolfInfo);
            _serverLifecycleManager.Stop(false, 0);
        }

        public new void Dispose()
        {
            _serverLifecycleManager.Dispose();
            base.Dispose();
        }
    }
}
