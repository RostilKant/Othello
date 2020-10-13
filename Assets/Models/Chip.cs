using UnityEngine;

namespace Models
{
    public class Chip
    {
        public Color? Color { get; internal set; }
        
        public Chip(Color color)
        {
            Color = color;
        }
    }
}