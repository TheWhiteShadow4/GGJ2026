# TWS Utilities

Ein Unity-Package mit nützlichen Utility-Funktionen und -Klassen für die Spieleentwicklung.

## Installation

Das Package kann über den Unity Package Manager installiert werden:

1. Öffne den Package Manager in Unity (Window > Package Manager)
2. Klicke auf das '+' Symbol und wähle "Add package from git URL..."
3. Füge die folgende URL ein: `https://github.com/TheWhiteShadow4/com.tws.utils.git`

Alternativ kannst du auch die folgende Zeile direkt in die `manifest.json` deines Projekts einfügen:

```json
{
  "dependencies": {
    "com.tws.utils": "https://github.com/TheWhiteShadow4/com.tws.utils.git"
  }
}
```

## Features

- **Object Pooling System** - Effizientes Pooling von GameObjects und Components mit automatischer Rückgabe bei Deaktivierung
- **Timer System** - Hochperformantes Timer-System mit verzögerten und wiederholenden Callbacks (bis zu 1024 gleichzeitige Timer)
- **Billboard System** - Optimiertes Billboard-System für 3D-Objekte, die zur Kamera ausgerichtet werden
- **Extension Methods** - Praktische Erweiterungen für Unity-Typen (LayerMask, Transform, Vector2/3, MethodInfo)
- **List Extensions** - Effiziente List-Operationen wie Shuffle und SwapRemove für bessere Performance
- **Serializable Dictionary** - Dictionary-Implementierung mit Unity-Serialisierung für Inspector-Unterstützung
- **CoroutineThread** - CustomYieldInstruction für Threading in Coroutines

## Verwendung

[Dokumentation folgt]

## Lizenz

Dieses Package steht unter der Mozilla Public License Version 2.0. Siehe [LICENSE.md](LICENSE.md) für Details.
