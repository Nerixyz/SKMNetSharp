# SKMNetSharp
SKMNet implementation in C#.

This project is in german.

### Nutzung
Das Projekt ist offensichtlich noch nicht fertig und deshalb gibt es noch Fehler bzw. Implementationslücken,
aber eine Grundlegende Verbindung funktioniert schon.

Um eine Verbindung zu initialisieren muss nur der Konstruktor von `LightingConsole` aufgerufen werden.
```
LightingConsole console = new LightingConsole(IP_ADRESSE);
```
Zur Zeit muss die IP-Adresse richtig sein (sonst gibt ein Socket einen Fehler).

Zum Testen empfehle ich den [NT Offline Editor](https://www.etcconnect.com/Products/Consoles/Legacy/Focus-NTX/Software.aspx).
Es ist auch die einzige Möglichkeit mit der man sich zZt. sicher verbinden kann.
