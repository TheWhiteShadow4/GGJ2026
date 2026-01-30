# TWS Settings

Ein Unity-Package für die Verwaltung von Spieleinstellungen und Konfigurationen.

## Installation

Das Package kann über den Unity Package Manager installiert werden:

1. Öffne den Package Manager in Unity (Window > Package Manager)
2. Klicke auf das '+' Symbol und wähle "Add package from git URL..."
3. Füge die folgende URL ein: `https://github.com/TheWhiteShadow4/com.tws.settings.git`

Alternativ kannst du auch die folgende Zeile direkt in die `manifest.json` deines Projekts einfügen:

```json
{
  "dependencies": {
    "com.tws.settings": "https://github.com/TheWhiteShadow4/com.tws.settings.git",
	"com.tws.utils": "https://github.com/TheWhiteShadow4/com.tws.utils.git"
  }
}
```

## Features

- **PlayerPrefs-basierte Persistierung** - Automatisches Speichern und Laden von Einstellungen
- **Smarte Setting-Komponenten** - Audio, Auflösung, Grafikqualität mit automatischer Übernahme der Werte
- **Bessere UI-Komponenten** - Prakmatische Erweiterung und Verbesserung der Builtin Klassen wie Karussell oder Drehknopf Elemente
- **Event-basierte Architektur** - Einheitliche Schittstellen für eigene Event Verarbeitung
- **Anpassbare Formatierung** - Eigene Formatierungsfunktionen für Werteanzeige (z.B. Prozent, Auflösungsformat)

# **Weitere Features**
- Reaktive Button Label
- Textanzeige für Slider

## Verwendung

[Dokumentation folgt]

## Lizenz

Dieses Package steht unter der Mozilla Public License Version 2.0. Siehe [LICENSE.md](LICENSE.md) für Details. 