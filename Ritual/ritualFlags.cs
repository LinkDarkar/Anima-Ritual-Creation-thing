namespace ritualFlags
{
    public enum TypesOfTime
    {
        NONE        = 0b_0000_0000,
        SECONDS     = 0b_0000_0001,
        MINUTES     = 0b_0000_0010,
        HOURS       = 0b_0000_0011,
        DAYS        = 0b_0000_0100,
        WEEKS       = 0b_0000_0101,
        MONTHS      = 0b_0000_0110,
        YEARS       = 0b_0000_0111,
        LIFETIMES   = 0b_0000_1000,
    }

    public enum CircumstanceComplexityModifiers
    {
        NONE                        = 0b_0000_0000,
        LOCATION_LEYLINE_ALIGNS     = 0b_0000_0001,
        LOCATION_LEYLINE_OPPOSES    = 0b_0000_0010,
        WEATHER_ALIGNMENT           = 0b_0000_0011,
        TIME_ALIGNMENT              = 0b_0000_0100,
        RITUAL_OPPOSES_NATURE       = 0b_0000_0101,
        THREEFOLD_SYMMETRY          = 0b_0000_0110,
        FIVEFOLD_SYMETRY            = 0b_0000_0111,
        LIFE_ON_THE_LINE            = 0b_0000_1000
    }

    public enum ComponentClass
    {
        NONE            = 0b_0000_0000,
        QUINTESSENCE    = 0b_0000_0001,
        AUGMENT         = 0b_0000_0010,
        STRUCTURE       = 0b_0000_0011,
        UTILITY         = 0b_0000_0100 
    }

    public enum Intensity
    {
        NONE            = 0b_0000_0000,
        HEAT            = 0b_0000_0001,
        COLD            = 0b_0000_0010,
        ELECTRICITY     = 0b_0000_0011
    }
}