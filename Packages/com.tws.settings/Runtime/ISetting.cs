/// <summary>
/// Marker Interface für erstmaliges Setzen von Einstellungen.
/// <br/>
/// Dieses Interface wird von allen Einstellungen implementiert, die einen Wert benötigen,
/// der erstmalig beim Start des Spiels gesetzt werden muss.
///
/// <example>
/// Das folgende Beispiel zeigt, wie man alle Einstellungen in einem Menü initialisiert.
/// Hierbei wird InitValue auch für inaktive Einstellungen aufgerufen. ACHTUNG: Dies kann noch vor Awake statt finden!
/// <code>
/// public class Menu : MonoBehaviour
/// {
///     public void Start()
///     {
///         foreach (var setting in GetComponentsInChildren<ISetting>(true))
///         {
///             setting.InitValue();
///         }
///     }
/// }
/// </code>
/// </example>
/// </summary>
public interface ISetting
{
	/// <summary>
	/// Setzt den Wert der Einstellung auf den Standardwert.
	/// </summary>
	void InitValue();
}