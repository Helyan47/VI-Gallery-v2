﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class PerfilClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public long numMenus { get; set; }
        

        public PerfilClass(long id,string nombre,long numMenus) {
            this.id = id;
            this.nombre = nombre;
            this.numMenus = numMenus;
        }

        public PerfilClass(string nombre) {
            this.id = -1;
            this.nombre = nombre;
        }

    }
}
