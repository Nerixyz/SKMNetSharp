# SKMNetSharp
SKMNet implementation in C#.

This project is in german.

## Testen
Das Projekt ist offensichtlich noch nicht fertig und deshalb gibt es noch Fehler bzw. Implementationslücken,
aber eine Grundlegende Verbindung funktioniert schon.

Um eine Verbindung aufzubauen:
```
LightingConsole console = new LightingConsole(IP_ADRESSE, LightingConsole.ConsoleSettings.All());
await console.ConnectAsync();
```
Zur Zeit muss die IP-Adresse richtig sein (sonst gibt ein Socket einen Fehler).

Zum Testen empfehle ich den [NT Offline Editor](https://www.etcconnect.com/Products/Consoles/Legacy/Focus-NTX/Software.aspx).

### Bekannte Fehler

- Die GUI hat zZt. keine Funktion
- Nicht alle Packets sind implementiert
- Pal- und ParSelect funktionieren noch nicht


## Nutzung

### Szenen

Alle *geladenen* Szenen werden in `LightingConsole.BLK` gespeichert, also als `MLPal`.
Man kann eine Szene mit `LightningConsole.BLK.Find((x) => x.Number == BLK_NUMBER)` auswählen bzw. `x.Name` oder anderen Eigenschaften, die eine MLPal besitzt. Achtung: `List<T>.Find(Predicate<T>)` kann `null` zurückgeben. Also sollte man immer überprüfen, ob die Szene "existiert".

Man kann Szenen mit `LightingConsole.CreateScene(string name, double number, Action<Enums.FehlerT> callback = null)` erstellen. `callback` kann null sein, da dann die Action einfach ignoriert wird. Ähnliche Funktionen für `edit, delete und rename` werden bald hinzugefügt.

