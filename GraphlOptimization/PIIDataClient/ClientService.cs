using System.Collections.Concurrent;

namespace PIIDataClient;

public class ClientService
{
    private static ConcurrentDictionary<Guid, Client> _clients = new ConcurrentDictionary<Guid, Client>();
    private static string[] _names;
    private static string[] _surnames;

    private static string[] _domains = new[]
    {
        "gmail.com",
        "wp.pl",
        "protonmail.com"
    };
    
    static ClientService()
    {
        var names = new List<string>();
        var surnames = new List<string>();
        var manifestNames = typeof(ClientService).Assembly.GetManifestResourceNames();
        var sourceFile = typeof(ClientService).Assembly.GetManifestResourceStream("PIIDataClient.Data.names.txt");
        var rows = new StreamReader(sourceFile);
        foreach (var line in rows.ReadToEnd().Split(Environment.NewLine))
        {
            var fields = line.Split(' ');
            names.Add(fields[0].Trim());
            surnames.Add(fields[0].Trim());
        }

        _names = names.ToArray();
        _surnames = names.ToArray();
    }
    

    public Client GetClientById(Guid clientId)
    {
        if (_clients.ContainsKey(clientId))
            return _clients[clientId];
        return InitializeClientData(clientId);
    }

    private Client InitializeClientData(Guid clientId)
    {
        var name = _names[Random.Shared.Next(0, _names.Length-1)];
        var surname = _surnames[Random.Shared.Next(0, _surnames.Length-1)];
        var domain = _domains[Random.Shared.Next(0, _domains.Length - 1)];
        var email = $"{name}.{surname.Substring(0, surname.Length / 2)}.{Random.Shared.Next(70, 99)}@{domain}";
        var client = new Client()
        {
            Name = name,
            Surname = surname,
            EmailAddress = email
        };
        _clients.TryAdd(clientId, client);
        return client;
    }
}