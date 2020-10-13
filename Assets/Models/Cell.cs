using UnityEngine;

namespace Models
{
    public class Cell
    {
        public Chip Chip { get; set; }
        
        public Cell(Color? color)
        {
            Chip.Color ??= color;
        }
    }
}