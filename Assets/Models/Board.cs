using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Board
    {
        public List<List<Cell>> Cells { get; set; }

        public Board()
        {
            int size = 8;
            Cells = new List<List<Cell>>(size);
            for (int i = 0; i < size; i++)
            {
                Cells.Add(new List<Cell>(size));
                for (int j = 0; j < size; j++)
                {
                    Cells[i].Add(new Cell(null));
                }
            }
            
            SetStartingChips();
        }

        private void SetStartingChips()
        {
            Cells[3][3].Chip = new Chip(Color.white);
            Cells[4][4].Chip = new Chip(Color.white);
            Cells[3][4].Chip = new Chip(Color.black);
            Cells[4][3].Chip = new Chip(Color.black);
        }
    }
}