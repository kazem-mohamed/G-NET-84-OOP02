#region Question 1
/*
a) Difference between a class and a struct:

   - A class is a reference type. Its object is stored on the heap, and variables
     hold references to that object. Copying a class variable copies the
     reference, so both variables point to the same object.

   - A struct is a value type. Its value is stored directly in the variable or
     inside the containing object. Copying a struct variable copies the whole
     value, so each variable has an independent copy.

   - Classes support inheritance, while structs cannot inherit from another
     struct or class.

b) Classes are more suitable than structs for large applications because large
   systems usually need inheritance, polymorphism, shared object identity, and
   controlled relationships between objects. Classes also avoid expensive copies
   when objects contain many fields, because only references are passed around.
*/
#endregion

#region Question 2
/*
a) The parent class is Shipment.

b) The child class is ExpressShipment.

c) ExpressShipment inherits all accessible members from Shipment, such as public
   and protected fields, properties, methods, and constructors' initialized
   state. Private members still exist inside the base part of the object, but
   they cannot be accessed directly from ExpressShipment.

d) Inheritance is better than duplicating code because common behavior is written
   once in the parent class and reused by child classes. This reduces repetition,
   makes maintenance easier, and keeps shared validation and business rules
   consistent across all shipment types.
*/
#endregion

#region Question 3
public struct DeliveryAddress
{
    public string City { get; set; }
    public string Street { get; set; }
    public int BuildingNumber { get; set; }

    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    public readonly string GetFullAddress()
    {
        return $"{BuildingNumber} {Street}, {City}";
    }
}

public class Shipment
{
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;

    public string TrackingCode
    {
        get => trackingCode;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Tracking code cannot be empty.");

            trackingCode = value;
        }
    }

    public string Description
    {
        get => description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Description cannot be empty.");

            description = value;
        }
    }

    public decimal Weight
    {
        get => weight;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Weight must be greater than zero.");

            weight = value;
        }
    }

    public decimal DeliveryFee
    {
        get => deliveryFee;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("Delivery fee must be greater than zero.");

            deliveryFee = value;
        }
    }

    public DeliveryAddress Destination { get; set; }

    public virtual decimal EstimatedCost => DeliveryFee + Weight * 5;

    public Shipment(string trackingCode)
        : this(
            trackingCode,
            "Unknown",
            1,
            50,
            new DeliveryAddress("Unknown", "Unknown", 0))
    {
    }

    public Shipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
    {
        TrackingCode = trackingCode;
        Description = description;
        Weight = weight;
        DeliveryFee = deliveryFee;
        Destination = destination;
    }

    public void UpdateDeliveryFee(decimal newFee)
    {
        DeliveryFee = newFee;
    }

    public virtual void PrintShipment()
    {
        Console.WriteLine($"Shipment Type: {GetType().Name}");
        Console.WriteLine($"Tracking Code: {TrackingCode}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Weight: {Weight} KG");
        Console.WriteLine($"Delivery Fee: {DeliveryFee} EGP");
        Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
        Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
    }
}

public class StandardShipment : Shipment
{
    public StandardShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
    }
}

public class ExpressShipment : Shipment
{
    private decimal extraFee;

    public decimal ExtraFee
    {
        get => extraFee;
        set
        {
            if (value < 0)
                throw new ArgumentException("Extra fee cannot be negative.");

            extraFee = value;
        }
    }

    public override decimal EstimatedCost => DeliveryFee + Weight * 5 + ExtraFee;

    public ExpressShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        decimal extraFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        ExtraFee = extraFee;
    }

    public override void PrintShipment()
    {
        base.PrintShipment();
        Console.WriteLine($"Extra Fee: {ExtraFee} EGP");
    }
}

public class InternationalShipment : Shipment
{
    private string destinationCountry;
    private decimal customsFee;

    public string DestinationCountry
    {
        get => destinationCountry;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Destination country cannot be empty.");

            destinationCountry = value;
        }
    }

    public decimal CustomsFee
    {
        get => customsFee;
        set
        {
            if (value < 0)
                throw new ArgumentException("Customs fee cannot be negative.");

            customsFee = value;
        }
    }

    public override decimal EstimatedCost => DeliveryFee + Weight * 5 + CustomsFee;

    public InternationalShipment(
        string trackingCode,
        string description,
        decimal weight,
        decimal deliveryFee,
        DeliveryAddress destination,
        string destinationCountry,
        decimal customsFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        DestinationCountry = destinationCountry;
        CustomsFee = customsFee;
    }

    public override void PrintShipment()
    {
        base.PrintShipment();
        Console.WriteLine($"Destination Country: {DestinationCountry}");
        Console.WriteLine($"Customs Fee: {CustomsFee} EGP");
    }
}

public class DeliveryCenter
{
    private const int Capacity = 20;
    private readonly Shipment[] shipments;

    public string CenterName { get; set; }

    public DeliveryCenter(string centerName)
    {
        CenterName = string.IsNullOrWhiteSpace(centerName)
            ? throw new ArgumentException("Center name cannot be empty.")
            : centerName;
        shipments = new Shipment[Capacity];
    }

    public Shipment this[int index]
    {
        get
        {
            if (index < 0 || index >= shipments.Length)
                return null;

            return shipments[index];
        }
        set
        {
            if (index < 0 || index >= shipments.Length)
                return;

            shipments[index] = value;
        }
    }

    public Shipment this[string trackingCode]
    {
        get
        {
            foreach (Shipment shipment in shipments)
            {
                if (shipment is not null && shipment.TrackingCode == trackingCode)
                    return shipment;
            }

            return null;
        }
    }

    public bool AddShipment(Shipment shipment)
    {
        if (shipment is null)
            return false;

        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] is null)
            {
                shipments[i] = shipment;
                return true;
            }
        }

        return false;
    }

    public bool RemoveShipment(string trackingCode)
    {
        for (int i = 0; i < shipments.Length; i++)
        {
            if (shipments[i] is not null && shipments[i].TrackingCode == trackingCode)
            {
                shipments[i] = null;
                return true;
            }
        }

        return false;
    }

    public void PrintAllShipments()
    {
        Console.WriteLine($"Delivery Center: {CenterName}");
        Console.WriteLine("--- Shipments ---");

        bool hasShipments = false;

        foreach (Shipment shipment in shipments)
        {
            if (shipment is null)
                continue;

            shipment.PrintShipment();
            Console.WriteLine();
            hasShipments = true;
        }

        if (!hasShipments)
            Console.WriteLine("No shipments found.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Smart Delivery Management System");
        Console.Write("Enter delivery center name: ");
        string centerName = ReadRequiredText();

        DeliveryCenter center = new DeliveryCenter(centerName);

        StandardShipment standardShipment = ReadStandardShipment();
        ExpressShipment expressShipment = ReadExpressShipment();
        InternationalShipment internationalShipment = ReadInternationalShipment();

        center.AddShipment(standardShipment);
        center.AddShipment(expressShipment);
        center.AddShipment(internationalShipment);

        Console.WriteLine();
        center.PrintAllShipments();

        Console.Write("Enter a tracking code to search: ");
        string searchCode = ReadRequiredText();
        Shipment foundShipment = center[searchCode];

        Console.WriteLine(foundShipment is null
            ? "Shipment not found."
            : $"Shipment found: {foundShipment.TrackingCode} - {foundShipment.Description}");

        Console.Write("Enter a tracking code to remove: ");
        string removeCode = ReadRequiredText();
        bool removed = center.RemoveShipment(removeCode);

        Console.WriteLine(removed
            ? "Shipment removed successfully."
            : "Shipment not found.");

        Console.WriteLine();
        center.PrintAllShipments();
    }

    private static StandardShipment ReadStandardShipment()
    {
        Console.WriteLine();
        Console.WriteLine("Enter Standard Shipment Data");
        ShipmentData data = ReadShipmentData();

        return new StandardShipment(
            data.TrackingCode,
            data.Description,
            data.Weight,
            data.DeliveryFee,
            data.Destination);
    }

    private static ExpressShipment ReadExpressShipment()
    {
        Console.WriteLine();
        Console.WriteLine("Enter Express Shipment Data");
        ShipmentData data = ReadShipmentData();
        decimal extraFee = ReadDecimal("Extra Fee: ", 0);

        return new ExpressShipment(
            data.TrackingCode,
            data.Description,
            data.Weight,
            data.DeliveryFee,
            data.Destination,
            extraFee);
    }

    private static InternationalShipment ReadInternationalShipment()
    {
        Console.WriteLine();
        Console.WriteLine("Enter International Shipment Data");
        ShipmentData data = ReadShipmentData();

        Console.Write("Destination Country: ");
        string destinationCountry = ReadRequiredText();
        decimal customsFee = ReadDecimal("Customs Fee: ", 0);

        return new InternationalShipment(
            data.TrackingCode,
            data.Description,
            data.Weight,
            data.DeliveryFee,
            data.Destination,
            destinationCountry,
            customsFee);
    }

    private static ShipmentData ReadShipmentData()
    {
        Console.Write("Tracking Code: ");
        string trackingCode = ReadRequiredText();

        Console.Write("Description: ");
        string description = ReadRequiredText();

        decimal weight = ReadDecimal("Weight: ", 0.01m);
        decimal deliveryFee = ReadDecimal("Delivery Fee: ", 0.01m);

        Console.Write("City: ");
        string city = ReadRequiredText();

        Console.Write("Street: ");
        string street = ReadRequiredText();

        int buildingNumber = ReadInt("Building Number: ");
        DeliveryAddress destination = new DeliveryAddress(city, street, buildingNumber);

        return new ShipmentData(
            trackingCode,
            description,
            weight,
            deliveryFee,
            destination);
    }

    private static string ReadRequiredText()
    {
        string input = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(input))
        {
            Console.Write("Please enter a valid value: ");
            input = Console.ReadLine();
        }

        return input;
    }

    private static decimal ReadDecimal(string prompt, decimal minimumValue)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();

        while (!decimal.TryParse(input, out decimal value) || value < minimumValue)
        {
            Console.Write($"Please enter a number greater than or equal to {minimumValue}: ");
            input = Console.ReadLine();
        }

        return decimal.Parse(input);
    }

    private static int ReadInt(string prompt)
    {
        Console.Write(prompt);
        string input = Console.ReadLine();

        while (!int.TryParse(input, out int value))
        {
            Console.Write("Please enter a valid integer number: ");
            input = Console.ReadLine();
        }

        return int.Parse(input);
    }

    private readonly struct ShipmentData
    {
        public ShipmentData(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee,
            DeliveryAddress destination)
        {
            TrackingCode = trackingCode;
            Description = description;
            Weight = weight;
            DeliveryFee = deliveryFee;
            Destination = destination;
        }

        public string TrackingCode { get; }
        public string Description { get; }
        public decimal Weight { get; }
        public decimal DeliveryFee { get; }
        public DeliveryAddress Destination { get; }
    }
}
#endregion
