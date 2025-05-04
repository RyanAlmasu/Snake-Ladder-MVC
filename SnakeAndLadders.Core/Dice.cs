using System;

namespace SnakeAndLadders.Core
{
    // Interface for dice implementations
    public interface IDice
    {
        // Rolls the dice and returns the result
        int Roll();
        
        // Gets the minimum possible value
        int MinValue { get; }
        
        // Gets the maximum possible value
        int MaxValue { get; }
    }

    // Standard dice implementation with values 1-6
    public class StandardDice : IDice
    {
        private readonly Random _random;
        
        public int MinValue => 1;
        public int MaxValue => 6;
        
        public StandardDice()
        {
            _random = new Random();
        }
        
        public StandardDice(int seed)
        {
            _random = new Random(seed);
        }
        
        public int Roll()
        {
            return _random.Next(MinValue, MaxValue + 1);
        }
    }
    
    // Custom dice implementation with configurable range
    public class CustomDice : IDice
    {
        private readonly Random _random;
        
        public int MinValue { get; }
        public int MaxValue { get; }
        
        public CustomDice(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("Min value must be less than max value");
                
            MinValue = minValue;
            MaxValue = maxValue;
            _random = new Random();
        }
        
        public CustomDice(int minValue, int maxValue, int seed)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("Min value must be less than max value");
                
            MinValue = minValue;
            MaxValue = maxValue;
            _random = new Random(seed);
        }
        
        public int Roll()
        {
            return _random.Next(MinValue, MaxValue + 1);
        }
    }
    
    // Fixed dice implementation for testing
    public class FixedDice : IDice
    {
        private readonly int _value;
        
        public int MinValue => _value;
        public int MaxValue => _value;
        
        public FixedDice(int value)
        {
            _value = value;
        }
        
        public int Roll()
        {
            return _value;
        }
    }
}
