# Mitwirken an der SKMNet implementation

### Issues
Wenn du Fehler meldest, folge bitte den Richtlinien.

Bei Exceptions füge bitte einen StackTrace deiner Beschreibung an z.B.:
```
at System.IO.MemoryStream.Read(Byte[] buffer, Int32 offset, Int32 count)
at SKMNET.ByteBuffer.ReadUShort() in SKMNET\Util\ByteBuffer.cs:line 38
at SKMNET.Client.Networking.Server.TSD.TSD_MLPal.ParsePacket(ByteBuffer buffer) in SKMNET\Client\Networking\Server\TSD\TSD_MLPal.cs:line 23
at SKMNET.Client.Networking.PacketDispatcher.OnDataIncoming(Byte[] data) in SKMNET\Client\Networking\PacketDispatcher.cs:line 98
```
Du kannst dir den StackTrace mit `exception.StackTrace` ausgeben lassen.
Füge zudem die Nachricht (`exception.Message`) hinzu und **einen Weg zur Reproduktion des Fehlers**!

### Pull Requests
Füge bitte einen Grund deiner PullRequest an.
Und halte dich zudem an die [C# Konventionen](https://docs.microsoft.com/de-de/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).
