﻿using Lidgren.Network;
using LunaCommon.Enums;
using LunaCommon.Message.Base;
using LunaCommon.Message.Types;
using System;

namespace LunaCommon.Message.Data.Handshake
{
    public class HandshakeReplyMsgData : HandshakeBaseMsgData
    {
        /// <inheritdoc />
        internal HandshakeReplyMsgData() { }
        public override HandshakeMessageType HandshakeMessageType => HandshakeMessageType.Reply;

        public HandshakeReply Response;
        public string Reason;
        public bool ModControl;
        public long ServerStartTime;
        public Guid  PlayerId;
        public string ModFileData;
        
        public override string ClassName { get; } = nameof(HandshakeReplyMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);

            lidgrenMsg.Write((int)Response);
            lidgrenMsg.Write(Reason);

            lidgrenMsg.Write(ModControl);
            lidgrenMsg.WritePadBits();

            lidgrenMsg.Write(ServerStartTime);

            GuidUtil.Serialize(PlayerId, lidgrenMsg);

            lidgrenMsg.Write(ModFileData);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);

            Response = (HandshakeReply)lidgrenMsg.ReadInt32();
            Reason = lidgrenMsg.ReadString();

            ModControl = lidgrenMsg.ReadBoolean();
            lidgrenMsg.SkipPadBits();

            ServerStartTime = lidgrenMsg.ReadInt64();
            
            PlayerId = GuidUtil.Deserialize(lidgrenMsg);

            ModFileData = lidgrenMsg.ReadString();
        }
        
        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + sizeof(HandshakeReply) + Reason.GetByteCount() + sizeof(byte) //We write pad bits so it's size of byte
                + sizeof(long) + GuidUtil.GetByteSize() + ModFileData.GetByteCount();
        }
    }
}