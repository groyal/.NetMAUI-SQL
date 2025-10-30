﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeebopNoteApp.Models
{
    public class NovuSettings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public bool Done { get; set; }

        public bool IsSelected { get; set; }

        public DateTime Date { get; set; }

    }
}