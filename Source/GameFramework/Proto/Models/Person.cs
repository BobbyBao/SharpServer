// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: Person.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace MasterServer
{

    [global::ProtoBuf.ProtoContract(Name = @"user_login_req_t")]
    public partial class UserLoginReqT : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"user_id", IsRequired = true)]
        public string UserId { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"user_name", IsRequired = true)]
        public string UserName { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"user_login_res_t")]
    public partial class UserLoginResT : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"res", IsRequired = true)]
        public int Res { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"user_id", IsRequired = true)]
        public string UserId { get; set; }

    }

    [global::ProtoBuf.ProtoContract(Name = @"message_type")]
    public enum MessageType
    {
        [global::ProtoBuf.ProtoEnum(Name = @"user_login_req")]
        UserLoginReq = 101,
        [global::ProtoBuf.ProtoEnum(Name = @"user_login_res")]
        UserLoginRes = 102,
    }

}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006
