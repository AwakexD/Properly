namespace Properly.Common;

public static class EntityConstants
{
    public class AddressConstants
    {
        public const int StreetNameMinLength = 8;
        public const int StreetNameMaxLength = 80;

        public const int CityNameMinLength = 3;
        public const int CityNameMaxLength = 30;

        public const int ZipCodeMinLength = 3;
        public const int ZipCodeMaxLength = 12;

        public const int CountryNameMinLength = 3;
        public const int CountryNameMaxLength = 20;

        public const double LatitudeMinLength = -90.0000000;
        public const double LatitudeMaxLength = 90.0000000;

        public const double LongitudeMinLength = -180.0000000;
        public const double LongitudeMaxLength = 180.0000000;
    }

    public class PropertyConstants
    {
        public const int SizeMinLength = 5;
        public const int SizeMaxLength = 30_000;

        public const int BathroomsMaxLength = 100;
        public const int BedroomsMaxLength = 100;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 5000;
    }
}
