namespace TechFood.BackOffice.Application.Common.Data;

public class Location
{
    public string Formatted
    {
        get
        {
            var street = Road;
            if (string.IsNullOrWhiteSpace(street))
            {
                street = Pedestrian;
            }

            if (!string.IsNullOrWhiteSpace(street) && street != "unnamed road")
            {
                if (!string.IsNullOrWhiteSpace(HouseNumber))
                {
                    return string.Concat(street, ", ", HouseNumber);
                }

                return string.Concat("Próximo a ", street);
            }
            else if (!string.IsNullOrWhiteSpace(Suburb))
            {
                return string.Concat("Próximo a ", Suburb);
            }

            return string.Empty;
        }
    }

    public string? Road { get; set; }

    public string? Pedestrian { get; set; }

    public string? Suburb { get; set; }

    public string? HouseNumber { get; set; }

    public string? Postcode { get; set; }

    public string? StateCode { get; set; }
}
