using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using GameFramework.Network;
using ProtoBuf;
using System.Net;
using System.Threading;
using GameFramework.Event;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Flower
{
    public class ProcedureNetSample : ProcedureBase
    {


        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }



        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("Begin Net Sample");

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnConnected);

            GameEntry.Event.Subscribe(Constant.EventDefine.ConnectGateServer, OnConnectGateServer);


            TestConnect();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnConnected);
            GameEntry.Event.Unsubscribe(Constant.EventDefine.ConnectGateServer, OnConnectGateServer);


        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }

        void TestConnect()
        {
            Debug.Log("-----------------Begin Connect--------------------:");
            //NetworkChannelHelper netHelper = new NetworkChannelHelper();
            ET_NetworkChannelHelper netHelper = new ET_NetworkChannelHelper();
            //GameEntry.Network.NetworkChannelHelper = netHelper;

            IPAddress ip = null;
            if (IPAddress.TryParse("127.0.0.1", out ip))
            {
                INetworkChannel nc = GameEntry.Network.CreateNetworkChannel("TC",0, netHelper);
                nc.HeartBeatInterval = 0f;

                nc.Connect(ip, 10002);
            }
        }

        void OnConnected(object sender, EventArgs e)
        {
            var temp = e as UnityGameFramework.Runtime.NetworkConnectedEventArgs;
            if (temp.NetworkChannel.Name.Equals("TC"))
            {
                Debug.Log("-----------连接服务器成功-----------:");
                INetworkChannel channel = GameEntry.Network.GetNetworkChannel("TC");
                //channel.Send(new CSMessageTest());
                Debug.Log("-----------开始发送登录请求-----------");
                channel.Send(new C2R_Login() { Account = "mdzz", Password = "zzdm" });
            }
            else if (temp.NetworkChannel.Name.Equals("CG_TC"))
            {
                Debug.Log("-----------连接服务器再次成功-----------:");
                INetworkChannel channel = GameEntry.Network.GetNetworkChannel("CG_TC");
                R2C_Login r2c_Login = temp.UserData as R2C_Login;
                //channel.Send(new CSMessageTest());
                channel.Send(new C2G_LoginGate() { Key = r2c_Login == null ? 0 : r2c_Login.Key });
            }

        }

        void OnConnectGateServer(object sender, EventArgs e)
        {
            Debug.Log("~~~~~~~~~~~~");
            R2C_Login loginResult = sender as R2C_Login;
            if (loginResult == null)
            {
                return;
            }
            NetworkComponent network = GameEntry.Network;
            INetworkChannelHelper helper = new ET_NetworkChannelHelper();
            INetworkChannel nc = network.CreateNetworkChannel("CG_TC",0, helper);
            nc.HeartBeatInterval = 0f;

            IPEndPoint ipPoint = NetworkHelper.ToIPEndPoint(loginResult.Address);
            nc.Connect(ipPoint.Address, ipPoint.Port, loginResult);
        }


    }
}

