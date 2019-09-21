Note: the following documentation is is german, as the original is.

# Im Aufbau

## Info
### Datentypen

| Name | C Typ | Länge in bytes |
|------:|-------:|----------------|
| ushort | `unsigned short` | 2 |
|  short | `short` | 2 |
| byte | `unsigned char` | 1 |
| short | `short` | 2 |

# Server ▶ Client

## Telegrammaufbau

| Länge   | Typ                 | Name               |Info         |
|:-------:|--------------------:|:-------------------|:------------|


## RMON
> 0..12

### Sync

Damit kann man von der T90 aus überpruefen ob der SK-Monitor
aktiv ist. Ausserdem bekommt man anstehende Keyboard Eingaben
und einen PC Reset signalisiert.

| Länge   | Typ                 | Name               |Info         |
|:-------:|--------------------:|:-------------------|:------------|
|2|ushort|id| 1|

### ScreenData

| Länge   | Typ                 | Name               |Info         |
|:-------:|--------------------:|:-------------------|:------------|
|2|ushort|id| 2|
|2|ushort|start| Position im Bildschirm. Links oben entspricht Position 0.|
|2|ushort|count| MAX = 733 Anzahl der folgenden Bildschirm Daten length.|
|`2 * count`| `ushort[count]`| data| Bildschirmdaten (Bit 15..8 Attribut, Bit 7..0 Zeichen.) |


### PalData

Palettendaten (Komplett-Telegramm)

| Länge   | Typ                 | Name               |Info         |
|:-------:|--------------------:|:-------------------|:------------|
|2|ushort|id| 3|
|8 * 64 | `VideoFarbe[N_HW_PALETTE]` | farbeintrag | Die einzelnen Farbeinträge in der Palette

**Konstanten:** 

|Name | Wert | Info|
|:---|:---|---|
|`N_HW_PALETTE` | 64 | Anzahl in der Hardware vorhandene Paletten
|`SKMON_PAL_BEF` | 8 | Befehls- und Meldezeile
| 


**VideoFarbe**

| Länge   | Typ                 | Name               |Info         |
|:-------:|--------------------:|:-------------------|:------------|
| 2 | short | farbno | Farb-Nummer 0...N_HW_PALETTE-1