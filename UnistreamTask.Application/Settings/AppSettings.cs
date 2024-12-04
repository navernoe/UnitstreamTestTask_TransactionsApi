namespace UnistreamTask.Application.Settings;

/// <summary>
/// Настройки приложения.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Ограничение на количество хранящихся транзакций.
    /// </summary>
    public int TransactionsCountLimit { get; set; }
}