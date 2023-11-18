var products = new Dictionary<string, (int quantity, float price, DateTime expiry)>
{
    ["Mlijeko"] = (4, 2.5f, (DateTime.Today.AddDays(-1))),
    ["Kruh"] = (2, 0.75f, (DateTime.Today.AddDays(1)))
};
var employees = new Dictionary<string, DateTime>
{
    ["Ivan Gundulić"] = new DateTime(1589, 1, 9),
    ["Andrej Plenković"] = new DateTime(1970, 4, 8)
};
var receipts = new Dictionary<int, (DateTime timeOfIssue, Dictionary<string, int> products)>
{
    [1] = (DateTime.Now, new Dictionary<string, int>
    {
        ["Mlijeko"] = 2,
        ["Kruh"] = 1
    })
};
const string password = "admin";

Console.WriteLine("Dobrodošli u program za upravljanje trgovinom!");
Console.WriteLine("U svakom trenutku možete unijeti Ctrl+Z za povratak u glavni izbornik.");

while (true)
{
    Console.WriteLine("1 - Artikli");
    Console.WriteLine("2 - Zaposlenici");
    Console.WriteLine("3 - Računi");
    Console.WriteLine("4 - Statistika");
    Console.WriteLine("0 - Izlaz");
    
    var input = Console.ReadLine();

    switch (input)
    {
        case "1": HandleProducts(); break;
        case "2": HandleEmployees(); break;
        case "3": HandleReceipts(); break;
        case "4": HandleStatistics(); break;
        case "0" or null: return;
    }
}

void HandleProducts()
{
    while (true)
    {
        Console.WriteLine("1 - Unos artikala");
        Console.WriteLine("2 - Brisanje artikala");
        Console.WriteLine("3 - Uređivanje artikala");
        Console.WriteLine("4 - Ispis artikala");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();

        switch (input)
        {
            case "1": AddProduct(); return;
            case "2": RemoveProduct(); return;
            case "3": EditProduct(); return;
            case "4": PrintProducts(); return;
            case "0" or null: return;
        }
    }
}

string FormatExpiry(DateTime expiry)
{   
    var daysLeft = (expiry - DateTime.Now).Days;
    return daysLeft switch
    {
        -1 => "istekao je jučer",
        < 0 => $"istekao je prije {-daysLeft} dana",
        0 => "ističe danas",
        1 => "ističe sutra",
        _ => $"ističe za {daysLeft} dana"
    };
}
    
void AddProduct()
{
    while (true)
    {
        Console.WriteLine("Unesite naziv novog artikla:");
        var name = Console.ReadLine();

        if (name == null) return;

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Naziv ne smije biti prazan!");
            continue;
        }

        if (products.ContainsKey(name))
        {
            Console.WriteLine("Artikl s tim nazivom već postoji!");
            continue;
        }

        Console.WriteLine("Unesite količinu:");
        var stringQuantity = Console.ReadLine();

        if (stringQuantity == null) return;

        var isNumber = int.TryParse(stringQuantity, out var quantity);

        if (!isNumber)
        {
            Console.WriteLine("Količina mora biti broj!");
            continue;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("Količina mora biti veća od 0!");
            continue;
        }

        Console.WriteLine("Unesite cijenu:");
        var stringPrice = Console.ReadLine();

        if (stringPrice == null) return;

        isNumber = float.TryParse(stringPrice, out var price);

        if (!isNumber)
        {
            Console.WriteLine("Cijena mora biti broj!");
            continue;
        }

        if (price <= 0)
        {
            Console.WriteLine("Cijena mora biti veća od 0!");
            continue;
        }

        Console.WriteLine("Unesite godinu isteka:");
        var stringYear = Console.ReadLine();

        if (stringYear == null) return;

        isNumber = int.TryParse(stringYear, out var year);

        if (!isNumber)
        {
            Console.WriteLine("Godina mora biti broj!");
            continue;
        }

        if (year is < 0 or > 9999)
        {
            Console.WriteLine("Godina mora biti između 0 i 9999!");
            continue;
        }

        Console.WriteLine("Unesite mjesec isteka:");
        var stringMonth = Console.ReadLine();
        
        if (stringMonth == null) return;
        
        isNumber = int.TryParse(stringMonth, out var month);
        
        if (!isNumber)
        {
            Console.WriteLine("Mjesec mora biti broj!");
            continue;
        }
        
        if (month is < 1 or > 12)
        {
            Console.WriteLine("Mjesec mora biti između 1 i 12!");
            continue;
        }
        
        Console.WriteLine("Unesite dan isteka:");
        var stringDay = Console.ReadLine();
        
        if (stringDay == null) return;
        
        isNumber = int.TryParse(stringDay, out var day);
        
        if (!isNumber)
        {
            Console.WriteLine("Dan mora biti broj!");
            continue;
        }
        
        if (day is < 1 || day > DateTime.DaysInMonth(year, month))
        {
            Console.WriteLine("Dan mora biti između 1 i broja dana u mjesecu!");
            continue;
        }
        
        var expiry = new DateTime(year, month, day);
        
        Console.WriteLine($"Jeste li sigurni da želite unijeti {name} (x{quantity} sa cijenom {price} sa istekom roka {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Unos artikla otkazan!");
            return;
        }
        
        products.Add(name, (quantity, price, expiry));
        Console.WriteLine("Artikl uspješno unesen!");
        return;
    }
}

void RemoveProduct()
{
    while (true)
    {
        Console.WriteLine("Unesite naziv artikla koji želite izbrisati:");
        var name = Console.ReadLine();

        if (name == null) return;

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Naziv ne smije biti prazan!");
            continue;
        }

        if (!products.ContainsKey(name))
        {
            Console.WriteLine("Artikl s tim nazivom ne postoji!");
            continue;
        }

        Console.WriteLine($"Jeste li sigurni da želite izbrisati {name}? (unesite \"da\" za potvrdu)");

        var confirmation = Console.ReadLine();

        if (confirmation == null) return;

        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Brisanje artikla otkazano!");
            return;
        }

        products.Remove(name);
        Console.WriteLine("Artikl uspješno izbrisan!");
        return;
    }
}

void EditProduct()
{
    while (true)
    {
       Console.WriteLine("1 - Zasebno uređivanje");
       Console.WriteLine("2 - Masovni popust/poskupljenje");
       
       var input = Console.ReadLine();

       switch (input)
       {
           case "1": EditProductIndividually(); return; 
           case "2": EditProductInBulk(); return;
       }
    }
}

void EditProductIndividually()
{
    while (true)
    {
        Console.WriteLine("Unesite naziv artikla koji želite urediti:");
        var name = Console.ReadLine();
        
        if (name == null) return;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Naziv ne smije biti prazan!");
            continue;
        }
        
        if (!products.ContainsKey(name))
        {
            Console.WriteLine("Artikl s tim nazivom ne postoji!");
            continue;
        }
        
        Console.WriteLine("1 - Uređivanje količine");
        Console.WriteLine("2 - Uređivanje cijene");
        Console.WriteLine("3 - Uređivanje datuma isteka");
        Console.WriteLine("4 - Uređivanje naziva");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1": EditProductQuantity(name); return;
            case "2": EditProductPrice(name); return;
            case "3": EditProductExpiry(name); return;
            case "4": EditProductName(name); return;
            case "0": return;
        }
    }
}

void EditProductQuantity(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novu količinu:");
        var stringQuantity = Console.ReadLine();

        if (stringQuantity == null) return;

        var isNumber = int.TryParse(stringQuantity, out var quantity);

        if (!isNumber)
        {
            Console.WriteLine("Količina mora biti broj!");
            continue;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("Količina mora biti veća od 0!");
            continue;
        }

        Console.WriteLine($"Jeste li sigurni da želite promijeniti količinu artikla {name} na {quantity}? (unesite \"da\" za potvrdu)");

        var confirmation = Console.ReadLine();

        if (confirmation == null) return;

        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje količine otkazano!");
            return;
        }

        products[name] = (quantity, products[name].price, products[name].expiry);
        Console.WriteLine("Količina uspješno promijenjena!");
        return;
    }
}

void EditProductPrice(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novu cijenu:");
        var stringPrice = Console.ReadLine();
        
        if (stringPrice == null) return;
        
        var isNumber = float.TryParse(stringPrice, out var price);
        
        if (!isNumber)
        {
            Console.WriteLine("Cijena mora biti broj!");
            continue;
        }
        
        if (price <= 0)
        {
            Console.WriteLine("Cijena mora biti veća od 0!");
            continue;
        }
        
        Console.WriteLine($"Jeste li sigurni da želite promijeniti cijenu artikla {name} na {price}? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje cijene otkazano!");
            return;
        }
        
        products[name] = (products[name].quantity, price, products[name].expiry);
        return;
    }
}

void EditProductExpiry(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novi datum isteka:");
        var stringYear = Console.ReadLine();

        if (stringYear == null) return;

        var isNumber = int.TryParse(stringYear, out var year);

        if (!isNumber)
        {
            Console.WriteLine("Godina mora biti broj!");
            continue;
        }

        if (year is < 0 or > 9999)
        {
            Console.WriteLine("Godina mora biti između 0 i 9999!");
            continue;
        }

        Console.WriteLine("Unesite novi mjesec isteka:");
        var stringMonth = Console.ReadLine();

        if (stringMonth == null) return;

        isNumber = int.TryParse(stringMonth, out var month);

        if (!isNumber)
        {
            Console.WriteLine("Mjesec mora biti broj!");
            continue;
        }

        if (month is < 1 or > 12)
        {
            Console.WriteLine("Mjesec mora biti između 1 i 12!");
            continue;
        }

        Console.WriteLine("Unesite novi dan isteka:");
        var stringDay = Console.ReadLine();

        if (stringDay == null) return;

        isNumber = int.TryParse(stringDay, out var day);

        if (!isNumber)
        {
            Console.WriteLine("Dan mora biti broj!");
            continue;
        }

        if (day is < 1 || day > DateTime.DaysInMonth(year, month))
        {
            Console.WriteLine("Dan mora biti između 1 i broja dana u mjesecu!");
            continue;
        }

        var expiry = new DateTime(year, month, day);

        Console.WriteLine(
            $"Jeste li sigurni da želite promijeniti datum isteka artikla {name} na {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})? (unesite \"da\" za potvrdu)");

        var confirmation = Console.ReadLine();

        if (confirmation == null) return;

        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje datuma isteka otkazano!");
            return;
        }
        
        products[name] = (products[name].quantity, products[name].price, expiry);
        return;
    }
}

void EditProductName(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novi naziv:");
        var newName = Console.ReadLine();

        if (newName == null) return;

        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("Naziv ne smije biti prazan!");
            continue;
        }

        if (products.ContainsKey(newName))
        {
            Console.WriteLine("Artikl s tim nazivom već postoji!");
            continue;
        }

        Console.WriteLine($"Jeste li sigurni da želite promijeniti naziv artikla {name} na {newName}? (unesite \"da\" za potvrdu)");

        var confirmation = Console.ReadLine();

        if (confirmation == null) return;

        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje naziva otkazano!");
            return;
        }

        products.Add(newName, products[name]);
        products.Remove(name);
        return;
    }
}

void EditProductInBulk()
{
    while (true)
    {
        Console.WriteLine("Unesite poskupljenje (pozitivan broj) ili popust (negativan broj):");
        var stringPrice = Console.ReadLine();
        
        if (stringPrice == null) return;
        
        var isNumber = float.TryParse(stringPrice, out var price);
        
        if (!isNumber)
        {
            Console.WriteLine("Cijena mora biti broj!");
            continue;
        }
        
        Console.WriteLine($"Jeste li sigurni da želite promijeniti cijenu svih artikala za {price}? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje cijene otkazano!");
            return;
        }
        
        foreach (var (name, (quantity, oldPrice, expiry)) in products)
        {
            products[name] = (quantity, oldPrice + price, expiry);
        }
        return;
    }
}

void PrintProducts()
{
    while (true)
    {
        Console.WriteLine("1 - Ispis svih artikala");
        Console.WriteLine("2 - Ispis svih artikala sortiranih po imenu");
        Console.WriteLine("3 - Ispis svih artikala sortiranih po datumu isteka roka (silazno)");
        Console.WriteLine("4 - Ispis svih artikala sortiranih po datumu isteka roka (uzlazno)");
        Console.WriteLine("5 - Ispis svih artikala sortiranih po kolčini");
        Console.WriteLine("6 - Ispis najprodavanijeg artikala");
        Console.WriteLine("7 - Ispis najmanje prodavanog artikla");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1": PrintAllProducts(); return;
            case "2": PrintAllProductsSortedByName(); return;
            case "3": PrintAllProductsSortedByExpiryDescending(); return;
            case "4": PrintAllProductsSortedByExpiryAscending(); return;
            case "5": PrintAllProductsSortedByQuantity(); return;
            case "6": PrintBestSellingProduct(); return;
            case "7": PrintWorstSellingProduct(); return;
            case "0": return;
        }
    }
}

void PrintAllProducts()
{
    foreach (var (name, (quantity, price, expiry)) in products)
    {
        Console.WriteLine($"{name} (x{quantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
    }
}

void PrintAllProductsSortedByName()
{
    foreach (var (name, (quantity, price, expiry)) in products.OrderBy(x => x.Key))
    {
        Console.WriteLine($"{name} (x{quantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
    }
}

void PrintAllProductsSortedByExpiryDescending()
{
    foreach (var (name, (quantity, price, expiry)) in products.OrderByDescending(x => x.Value.expiry))
    {
        Console.WriteLine($"{name} (x{quantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
    }
}

void PrintAllProductsSortedByExpiryAscending()
{
    foreach (var (name, (quantity, price, expiry)) in products.OrderBy(x => x.Value.expiry))
    {
        Console.WriteLine($"{name} (x{quantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
    }
}

void PrintAllProductsSortedByQuantity()
{
    foreach (var (name, (quantity, price, expiry)) in products.OrderBy(x => x.Value.quantity))
    {
        Console.WriteLine($"{name} (x{quantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
    }
}

void PrintBestSellingProduct()
{
    var productsSellQuantity = new Dictionary<string, int>();
    foreach (var receipt in receipts)
    { 
        foreach (var product in receipt.Value.products)
        {
            if (productsSellQuantity.ContainsKey(product.Key))
            {
                productsSellQuantity[product.Key] += product.Value;
            }
            else
            {
                productsSellQuantity.Add(product.Key, product.Value);
            }
        }
    }
    
    var bestSellingProduct = productsSellQuantity.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    Console.WriteLine($"{bestSellingProduct} je najprodavaniji artikl sa {productsSellQuantity[bestSellingProduct]} prodanih komada.");
}

void PrintWorstSellingProduct()
{
    var productsSellQuantity = new Dictionary<string, int>();
    foreach (var receipt in receipts)
    { 
        foreach (var product in receipt.Value.products)
        {
            if (productsSellQuantity.ContainsKey(product.Key))
            {
                productsSellQuantity[product.Key] += product.Value;
            }
            else
            {
                productsSellQuantity.Add(product.Key, product.Value);
            }
        }
    }
    
    var worstSellingProduct = productsSellQuantity.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
    Console.WriteLine($"{worstSellingProduct} je najgore prodavaniji artikl sa {productsSellQuantity[worstSellingProduct]} prodanih komada.");
}

void HandleEmployees()
{
    while (true)
    {
        Console.WriteLine("1 - Unos radnika");
        Console.WriteLine("2 - Brisanje radnika");
        Console.WriteLine("3 - Uređivanje radnika");
        Console.WriteLine("4 - Ispis radnika");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();

        switch (input)
        {
            case "1": AddEmployee(); return;
            case "2": RemoveEmployee(); return;
            case "3": EditEmployee(); return;
            case "4": PrintEmployees(); return;
            case "0" or null: return;
        }
    }
}

int CalculateAge(DateTime birthDate)
{
    var age = DateTime.Now.Year - birthDate.Year;
    if (DateTime.Now.DayOfYear < birthDate.DayOfYear) age--;
    return age;
}

void AddEmployee()
{
    while (true)
    {
        Console.WriteLine("Unesite ime i prezime novog radnika:");
        var name = Console.ReadLine();
        
        if (name == null) return;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ime ne smije biti prazno!");
            continue;
        }
        
        if (employees.ContainsKey(name))
        {
            Console.WriteLine("Radnik s tim imenom već postoji!");
            continue;
        }
        
        Console.WriteLine("Unesite godinu rođenja:");
        var stringYear = Console.ReadLine();
        
        if (stringYear == null) return;
        
        var isNumber = int.TryParse(stringYear, out var year);
        
        if (!isNumber)
        {
            Console.WriteLine("Godina mora biti broj!");
            continue;
        }
        
        if (year is < 0 or > 9999)
        {
            Console.WriteLine("Godina mora biti između 0 i 9999!");
            continue;
        }
        
        Console.WriteLine("Unesite mjesec rođenja:");
        
        var stringMonth = Console.ReadLine();
        
        if (stringMonth == null) return;
        
        isNumber = int.TryParse(stringMonth, out var month);
        
        if (!isNumber)
        {
            Console.WriteLine("Mjesec mora biti broj!");
            continue;
        }
        
        if (month is < 1 or > 12)
        {
            Console.WriteLine("Mjesec mora biti između 1 i 12!");
            continue;
        }
        
        Console.WriteLine("Unesite dan rođenja:");
        var stringDay = Console.ReadLine();
        
        if (stringDay == null) return;
        
        isNumber = int.TryParse(stringDay, out var day);
        
        if (!isNumber)
        {
            Console.WriteLine("Dan mora biti broj!");
            continue;
        }
        
        if (day is < 1 || day > DateTime.DaysInMonth(year, month))
        {
            Console.WriteLine("Dan mora biti između 1 i broja dana u mjesecu!");
            continue;
        }
        
        var birthDate = new DateTime(year, month, day);
        
        Console.WriteLine($"Jeste li sigurni da želite unijeti {name} (rođen {birthDate:dd/MM/yyy} ({CalculateAge(birthDate)}))? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Unos radnika otkazan!");
            return;
        }
        
        employees.Add(name, birthDate);
        
        Console.WriteLine("Radnik uspješno unesen!");   
        return;
    }
}

void RemoveEmployee()
{
    while (true)
    {
        Console.WriteLine("1 - Brisanje radnika po imenu");
        Console.WriteLine("2 - Brisanje svih radnika starijih od 65 godina");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1": RemoveEmployeeByName(); return;
            case "2": RemoveRetiredEmployees(); return;
            case "0" or null: return;
        }
    }
}

void RemoveEmployeeByName()
{
    while (true)
    {
        Console.WriteLine("Unesite ime radnika kojeg želite izbrisati:");
        var name = Console.ReadLine();
        
        if (name == null) return;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ime ne smije biti prazno!");
            continue;
        }
        
        if (!employees.ContainsKey(name))
        {
            Console.WriteLine("Radnik s tim imenom ne postoji!");
            continue;
        }
        
        Console.WriteLine($"Jeste li sigurni da želite izbrisati {name}? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Brisanje radnika otkazano!");
            return;
        }
        
        employees.Remove(name);
        Console.WriteLine("Radnik uspješno izbrisan!");
        return;
    }
}

void RemoveRetiredEmployees()
{
    foreach (var employee in employees.Where(x => x.Value.AddYears(65) < DateTime.Now).ToList())
    {
        employees.Remove(employee.Key);
    }
    
    Console.WriteLine("Radnici uspješno izbrisani!");
}

void EditEmployee()
{
    while (true)
    {
        Console.WriteLine("1 - Uređivanje imena");
        Console.WriteLine("2 - Uređivanje datuma rođenja");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();
        
        Console.WriteLine("Unesite ime radnika kojeg želite urediti:");
        var name = Console.ReadLine();
        
        if (name == null) return;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Ime ne smije biti prazno!");
            continue;
        }
        
        if (!employees.ContainsKey(name))
        {
            Console.WriteLine("Radnik s tim imenom ne postoji!");
            continue;
        }
        
        switch (input)
        {
            case "1": EditEmployeeName(name); return;
            case "2": EditEmployeeBirthDate(name); return;
            case "0" or null: return;
        }
    }
}

void EditEmployeeName(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novo ime:");
        var newName = Console.ReadLine();
        
        if (newName == null) return;
        
        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("Ime ne smije biti prazno!");
            continue;
        }
        
        if (employees.ContainsKey(newName))
        {
            Console.WriteLine("Radnik s tim imenom već postoji!");
            continue;
        }
        
        Console.WriteLine($"Jeste li sigurni da želite promijeniti ime radnika {name} na {newName}? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje imena otkazano!");
            return;
        }
        
        employees.Add(newName, employees[name]);
        employees.Remove(name);
        Console.WriteLine("Ime uspješno promijenjeno!");
        return;
    }
}

void EditEmployeeBirthDate(string name)
{
    while (true)
    {
        Console.WriteLine("Unesite novi datum rođenja:");
        var stringYear = Console.ReadLine();

        if (stringYear == null) return;

        var isNumber = int.TryParse(stringYear, out var year);

        if (!isNumber)
        {
            Console.WriteLine("Godina mora biti broj!");
            continue;
        }

        if (year is < 0 or > 9999)
        {
            Console.WriteLine("Godina mora biti između 0 i 9999!");
            continue;
        }

        Console.WriteLine("Unesite novi mjesec rođenja:");
        var stringMonth = Console.ReadLine();

        if (stringMonth == null) return;

        isNumber = int.TryParse(stringMonth, out var month);

        if (!isNumber)
        {
            Console.WriteLine("Mjesec mora biti broj!");
            continue;
        }

        if (month is < 1 or > 12)
        {
            Console.WriteLine("Mjesec mora biti između 1 i 12!");
            continue;
        }

        Console.WriteLine("Unesite novi dan rođenja:");
        var stringDay = Console.ReadLine();

        if (stringDay == null) return;

        isNumber = int.TryParse(stringDay, out var day);

        if (!isNumber)
        {
            Console.WriteLine("Dan mora biti broj!");
            continue;
        }

        if (day is < 1 || day > DateTime.DaysInMonth(year, month))
        {
            Console.WriteLine("Dan mora biti između 1 i broja dana u mjesecu!");
            continue;
        }

        var birthDate = new DateTime(year, month, day);

        Console.WriteLine(
            $"Jeste li sigurni da želite promijeniti datum rođenja radnika {name} na {birthDate:dd/MM/yyy} ({CalculateAge(birthDate)})? (unesite \"da\" za potvrdu)");

        var confirmation = Console.ReadLine();

        if (confirmation == null) return;

        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Uređivanje datuma rođenja otkazano!");
            return;
        }
        
        employees[name] = birthDate;
        Console.WriteLine("Datum rođenja uspješno promijenjen!");
        return;
    }
}

void PrintEmployees()
{
    while (true)
    {
        Console.WriteLine("1 - Ispis svih radnika");
        Console.WriteLine("2 - Ispis svih radnika sa rođendanom u tekućem mjesecu");
        
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1": PrintAllEmployees(); return;
            case "2": PrintEmployeesWithBirthdayThisMonth(); return;
        }
    }
}

void PrintAllEmployees()
{
    foreach (var (name, birthDate) in employees)
    {
        Console.WriteLine($"{name} ({CalculateAge(birthDate)} godina) - {birthDate:dd/MM/yyy}");
    }
}

void PrintEmployeesWithBirthdayThisMonth()
{
    foreach (var (name, birthDate) in employees.Where(x => x.Value.Month == DateTime.Now.Month))
    {
        Console.WriteLine($"{name} ({CalculateAge(birthDate)} godina) - {birthDate:dd/MM/yyy}");
    }
}

void HandleReceipts()
{
    while (true)
    {
        Console.WriteLine("1 - Unos računa");
        Console.WriteLine("2 - Ispis računa");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();

        switch (input)
        {
            case "1": AddReceipt(); return;
            case "2": PrintReceipts(); return;
            case "0" or null: return;
        }
    }
}

void AddReceipt()
{
    while (true)
    {
        var receiptProducts = new Dictionary<string, int>();
        while (true)
        {
            Console.WriteLine("Dostupni artikli:");
            foreach (var (productName, (productQuantity, price, expiry)) in products)
            {
                Console.WriteLine($"{productName} (x{productQuantity}) - {price} eur - {expiry:dd/MM/yyy} ({FormatExpiry(expiry)})");
            }
        
            Console.WriteLine("Unesite naziv artikla ili \"kraj\":");
            var name = Console.ReadLine();
        
            if (name == null) return;
        
            if (name.ToLower() == "kraj")
            {
                Console.WriteLine("Unos računa završen!");
                break;
            }
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Naziv ne smije biti prazan!");
                continue;
            }
            
            if (!products.ContainsKey(name))
            {
                Console.WriteLine("Artikl s tim nazivom ne postoji!");
                continue;
            }
            
            Console.WriteLine("Unesite količinu:");
            
            var stringQuantity = Console.ReadLine();
            
            if (stringQuantity == null) return;
            
            var isNumber = int.TryParse(stringQuantity, out var quantity);
            
            if (!isNumber)
            {
                Console.WriteLine("Količina mora biti broj!");
                continue;
            }
            
            if (quantity <= 0)
            {
                Console.WriteLine("Količina mora biti veća od 0!");
                continue;
            }
            
            if (quantity > products[name].quantity)
            {
                Console.WriteLine("Nema dovoljno artikala na stanju!");
                continue;
            }
            
            if (receiptProducts.ContainsKey(name))
            {
                receiptProducts[name] += quantity;
            }
            else
            {
                receiptProducts.Add(name, quantity);
            }
            
            Console.WriteLine("Artikl uspješno dodan!");
        }
        Console.WriteLine("Jeste li sigurni da želite unijeti ovaj račun? (unesite \"da\" za potvrdu)");
        
        var confirmation = Console.ReadLine();
        
        if (confirmation == null) return;
        
        if (confirmation.ToLower() != "da")
        {
            Console.WriteLine("Unos računa otkazan!");
            return;
        }
        
        var receiptId = receipts.Count + 1;
        receipts.Add(receiptId, (DateTime.Now, receiptProducts));

        var receiptTime = DateTime.Now;

        foreach (var product in receiptProducts)
        {
            var databaseProduct = products[product.Key];
            databaseProduct.quantity -= product.Value;
            
            if (databaseProduct.quantity == 0)
            {
                products.Remove(product.Key);
            }
        }
        
        Console.WriteLine($"Račun {receiptId}:");
        foreach (var (productName, quantity) in receiptProducts)
        {
            Console.WriteLine($"{productName} (x{quantity})");
        }
        Console.WriteLine(receiptTime.ToString("mm:HH, dd/MM/yyy"));
        return;
    }
}

void PrintReceipts()
{
    while (true)
    {
        Console.WriteLine("1 - Ispis svih računa");
        Console.WriteLine("2 - Ispis računa po ID-u");
        Console.WriteLine("0 - Povratak u glavni izbornik");
        
        var input = Console.ReadLine();
        
        switch (input)
        {
            case "1": PrintAllReceipts(); return;
            case "2": PrintReceiptById(); return;
            case "0" or null: return;
        }
    }
}

void PrintAllReceipts()
{
    foreach (var receipt in receipts)
    {
        var totalValue = 0f;
        foreach (var product in receipt.Value.products)
        {
            totalValue += products[product.Key].price * product.Value;
        }
        Console.WriteLine($"Račun {receipt.Key} izdan {receipt.Value.timeOfIssue:dd/MM/yyy} u {receipt.Value.timeOfIssue:HH:mm} sa ukupnom vrijednosti {totalValue} eur");
    }
}

void PrintReceiptById()
{
    while (true)
    {
        Console.WriteLine("Unesite ID računa:");
        var stringId = Console.ReadLine();
        
        if (stringId == null) return;
        
        var isNumber = int.TryParse(stringId, out var id);
        
        if (!isNumber)
        {
            Console.WriteLine("ID mora biti broj!");
            continue;
        }
        
        if (id <= 0)
        {
            Console.WriteLine("ID mora biti veći od 0!");
            continue;
        }
        
        if (!receipts.ContainsKey(id))
        {
            Console.WriteLine("Račun s tim ID-om ne postoji!");
            continue;
        }
        
        var receipt = receipts[id];
        
        var totalValue = 0f;
        foreach (var product in receipt.products)
        {
            totalValue += products[product.Key].price * product.Value;
        }
        Console.WriteLine($"Račun {id} izdan {receipt.timeOfIssue:dd/MM/yyy} u {receipt.timeOfIssue:HH:mm} sa ukupnom vrijednošću {totalValue}:");
        foreach (var product in receipt.products)
        {
            Console.WriteLine($"{product.Key} (x{product.Value})");
        }
        return;
    }
}

void HandleStatistics()
{
    while (true)
    {
        Console.WriteLine("Unesite lozinku:");
        var inputtedPassword = Console.ReadLine();
        
        if (inputtedPassword == null) return;
        
        if (inputtedPassword != password)
        {
            Console.WriteLine("Pogrešna lozinka!");
            continue;
        }

        while (true)
        {
            Console.WriteLine("1 - Ispis ukupnog broja artikala na stanju");
            Console.WriteLine("2 - Ispis ukupne vrijednosti artikala na stanju");
            Console.WriteLine("3 - Ispis ukupne vrijednosti prodanih artikala");
            Console.WriteLine("4 - Ispis prometa po mjesecima");
            Console.WriteLine("0 - Povratak u glavni izbornik");
            
            var input = Console.ReadLine();
            
            switch (input)
            {
                case "1": PrintTotalQuantity(); return;
                case "2": PrintTotalValue(); return;
                case "3": PrintTotalSoldValue(); return;
                case "4": PrintRevenueByMonth(); return;
                case "0" or null: return;
            }
        }
    }
}

void PrintTotalQuantity()
{
    var totalQuantity = 0;
    foreach (var (_, (quantity, _, _)) in products)
    {
        totalQuantity += quantity;
    }
    Console.WriteLine($"Ukupan broj artikala na stanju: {totalQuantity}");
}

void PrintTotalValue()
{
    var totalValue = 0f;
    foreach (var (_, (quantity, price, _)) in products)
    {
        totalValue += quantity * price;
    }
    Console.WriteLine($"Ukupna vrijednost artikala na stanju: {totalValue} eur");
}

void PrintTotalSoldValue()
{
    var totalValue = 0f;
    foreach (var receipt in receipts)
    {
        foreach (var product in receipt.Value.products)
        {
            totalValue += products[product.Key].price * product.Value;
        }
    }
    Console.WriteLine($"Ukupna vrijednost prodanih artikala: {totalValue} eur");
}

void PrintRevenueByMonth()
{
    var totalRevenue = 0f;
    while (true)
    {
        Console.WriteLine("Unesite godinu:");
        var stringYear = Console.ReadLine();
        
        if (stringYear == null) return;
        
        var isNumber = int.TryParse(stringYear, out var year);
        
        if (!isNumber)
        {
            Console.WriteLine("Godina mora biti broj!");
            continue;
        }
        
        if (year is < 0 or > 9999)
        {
            Console.WriteLine("Godina mora biti između 0 i 9999!");
            continue;
        }
        
        Console.WriteLine("Unesite mjesec:");
        var stringMonth = Console.ReadLine();
        
        if (stringMonth == null) return;
        
        isNumber = int.TryParse(stringMonth, out var month);
        
        if (!isNumber)
        {
            Console.WriteLine("Mjesec mora biti broj!");
            continue;
        }
        
        if (month is < 1 or > 12)
        {
            Console.WriteLine("Mjesec mora biti između 1 i 12!");
            continue;
        }
        
        Console.WriteLine("Unesite ukupan iznos plaće radnika:");
        var stringSalary = Console.ReadLine();
        
        if (stringSalary == null) return;
        
        isNumber = float.TryParse(stringSalary, out var salary);
        
        if (!isNumber)
        {
            Console.WriteLine("Plaća mora biti broj!");
            continue;
        }
        
        if (salary <= 0)
        {
            Console.WriteLine("Plaća mora biti veća od 0!");
            continue;
        }
        
        Console.WriteLine("Unosite iznos najma prostora:");
        var stringRent = Console.ReadLine();
        
        if (stringRent == null) return;
        
        isNumber = float.TryParse(stringRent, out var rent);
        
        if (!isNumber)
        {
            Console.WriteLine("Najam mora biti broj!");
            continue;
        }
        
        if (rent <= 0)
        {
            Console.WriteLine("Najam mora biti veći od 0!");
            continue;
        }
        
        Console.WriteLine("Unosite iznos svih ostalih troškova:");
        
        var stringOtherCosts = Console.ReadLine();
        
        if (stringOtherCosts == null) return;
        
        isNumber = float.TryParse(stringOtherCosts, out var otherCosts);
        
        if (!isNumber)
        {
            Console.WriteLine("Troškovi moraju biti broj!");
            continue;
        }
        
        if (otherCosts < 0)
        {
            Console.WriteLine("Troškovi moraju biti veći ili jednaki 0!");
            continue;
        }
        
        totalRevenue -= salary + rent + otherCosts;
        
        foreach (var receipt in receipts.Where(x => x.Value.timeOfIssue.Year == year && x.Value.timeOfIssue.Month == month))
        {
            foreach (var product in receipt.Value.products)
            {
                totalRevenue += products[product.Key].price * product.Value;
            }
        }
        
        Console.WriteLine($"Promet u {month}/{year} je {totalRevenue} eur");
        return;
    }
}