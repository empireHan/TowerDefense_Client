@echo off
@Rem protoc.exe --csharp_out="./Assets/Model/Module/Message/" --proto_path="../Proto/" OuterMessage.proto
protoc.exe --csharp_out="./Assets/GameMain/Scripts/Network/ETNetwork/MessageOutput/" --proto_path="./Assets/GameMain/Scripts/Network/ETNetwork/ProtoDefine/" HotfixMessage.proto
echo finish... 
pause