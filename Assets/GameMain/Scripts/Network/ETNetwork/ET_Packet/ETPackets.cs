using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProtoBuf;

namespace Flower
{
    public static class ETPackets
    {
        public static readonly int ET_PacketSizeLength = 2;

        public static readonly int ET_MessageIdentifyLength = 3;
        public static readonly int ET_MessageFlagIndex = 2;
        public static readonly int ET_MessageOpcodeIndex = 3;

    }

    //[Serializable, ProtoContract(Name = @"C2R_Login")]
    public partial class C2R_Register : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10013;
            }
        }

        public override void Clear()
        {

        }
    }

    //[Serializable,ProtoContract(Name = @"R2C_Login")]
    public partial class R2C_Register : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10014;
            }
        }

        public override void Clear()
        {

        }
    }

    //[Serializable, ProtoContract(Name = @"C2R_Login")]
    public partial class C2R_Login : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10001;
            }
        }
        
        public override void Clear()
        {
            
        }
    }

    //[Serializable,ProtoContract(Name = @"R2C_Login")]
    public partial class R2C_Login : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10002;
            }
        }

        public override void Clear()
        {
            
        }
    }

    public partial class C2G_LoginGate : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10003;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class G2C_LoginGate : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10004;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class TestMsg : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10011;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class GFTestMsg : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10012;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class G2C_TestHotfixMessage : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10005;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class C2M_TestActorRequest : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10006;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class M2C_TestActorResponse : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10007;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class PlayerInfo : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10008;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class C2G_PlayerInfo : CSPacketBase
    {
        public override int Id
        {
            get
            {
                return 10009;
            }
        }

        public override void Clear()
        {

        }
    }

    public partial class G2C_PlayerInfo : SCPacketBase
    {
        public override int Id
        {
            get
            {
                return 10010;
            }
        }

        public override void Clear()
        {

        }
    }


}
