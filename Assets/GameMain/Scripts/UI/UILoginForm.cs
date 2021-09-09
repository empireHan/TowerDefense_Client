using Flower.Data;
using GameFramework.DataTable;
using GameFramework.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Flower
{
    public class UILoginForm : UGuiFormEx
    {
        public Button registerButton;
        public Button loginButton;
        public Text account;
        public Text password;

        private bool isConnect = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            registerButton.onClick.AddListener(OnRegisterButtonClick);
            loginButton.onClick.AddListener(OnLoginButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Begin Net Sample");

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnConnected);

            GameEntry.Event.Subscribe(Constant.EventDefine.ConnectGateServer, OnConnectGateServer);

            GameEntry.Event.Subscribe(Constant.EventDefine.LevelConfigNotify, OnLevelConfigNotify);

            GameEntry.Event.Subscribe(Constant.EventDefine.RegisterNotify, OnRegisterNotify);

            GameEntry.Event.Subscribe(Constant.EventDefine.LoginGateSuccessNotify, OnLoginGateSuccess);


            TestConnect();

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.NetworkConnectedEventArgs.EventId, OnConnected);
            GameEntry.Event.Unsubscribe(Constant.EventDefine.ConnectGateServer, OnConnectGateServer);
            GameEntry.Event.Unsubscribe(Constant.EventDefine.LevelConfigNotify, OnLevelConfigNotify);
            GameEntry.Event.Unsubscribe(Constant.EventDefine.RegisterNotify, OnRegisterNotify);
            GameEntry.Event.Unsubscribe(Constant.EventDefine.LoginGateSuccessNotify, OnLoginGateSuccess);
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
                INetworkChannel nc = GameEntry.Network.CreateNetworkChannel("TC", 0, netHelper);
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
                isConnect = true;
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
            if(loginResult.Error != ErrorCode.ERR_Success)
            {
                Debug.Log($"Login failed! {loginResult.Message}");
                return;
            }
            NetworkComponent network = GameEntry.Network;
            INetworkChannelHelper helper = new ET_NetworkChannelHelper();
            INetworkChannel nc = network.CreateNetworkChannel("CG_TC", 0, helper);
            nc.HeartBeatInterval = 0f;

            IPEndPoint ipPoint = NetworkHelper.ToIPEndPoint(loginResult.Address);
            nc.Connect(ipPoint.Address, ipPoint.Port, loginResult);
        }

        void OnLevelConfigNotify(object sender, EventArgs e)
        {
#if DATA_FROM_SERVER
            Debug.LogError("recv from server LevelConfig Data~~~~~~~~~~~~~");
            R2C_LevelConfig levelConfigResult = sender as R2C_LevelConfig;
            DataTableBase dataTableBase = GameEntry.DataTable.GetDataTable("Level");
            foreach (LevelConfig lc in levelConfigResult.LevelConfigs)
            {
                string data = lc.Config.Replace('|', '\t');
                dataTableBase.ParseData(data);
                //Debug.LogError("recv from server LevelConfig Data:" + lc.Config);
            }
            //网络数据解析后，DataLevel初始化
            GameEntry.Data.GetData<DataLevel>().initData();
            //跳转UI   
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));
#endif
        }

        void OnRegisterNotify(object sender,EventArgs e)
        {
            R2C_Register registerResult = sender as R2C_Register;
            if(registerResult == null)
            {
                return;
            }
            if(registerResult.Error != ErrorCode.ERR_Success)
            {
                Debug.Log($"Register failed! {registerResult.Message}");
                return;
            }
            Debug.Log($"Register success! {registerResult.Message}");
        }

        void OnLoginGateSuccess(object sender, EventArgs e)
        {
            //G2C_LoginGate loginGate = sender as G2C_LoginGate;
            //跳转UI   
            //GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));
        }

        private void OnRegisterButtonClick()
        {
            if (isConnect)
            {
                INetworkChannel channel = GameEntry.Network.GetNetworkChannel("TC");
                Debug.Log("-----------开始发送注册请求-----------");
                channel.Send(new C2R_Register() { Account = account.text, Password = password.text });
            }
            else
            {
                Debug.Log("未连接到服务器，重连中...");
                TestConnect();
            }
        }

        private void OnLoginButtonClick()
        {
            if (isConnect)
            {
                INetworkChannel channel = GameEntry.Network.GetNetworkChannel("TC");
                Debug.Log("-----------开始发送登录请求-----------");
                channel.Send(new C2R_Login() { Account = account.text, Password = password.text });
            }
            else
            {
                Debug.Log("未连接到服务器，重连中...");
                TestConnect();
            }
        }

        private void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

    }
}


