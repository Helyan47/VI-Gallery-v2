﻿using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF {
    public class CarpetaClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaPrograma { get; set; }
        public string rutaPadre { get; set; }
        public string desc { get; set; }
        public string img { get; set; }
        public bool esCarpeta { get; set; }
        public long menu { get; set; }
        public ICollection<string> generos { get; set; }
        public int numSubCarpetas { get; set; }
        public int numArchivos { get; set; }

        public CarpetaClass(string title, string desc, string dirImg, ICollection<string> generos) {

            this.nombre = title;
            this.desc = desc;
            this.img = dirImg;
            this.generos = generos;
            this.numSubCarpetas = 0;
            this.numArchivos = 0;
        }
        public CarpetaClass(string title, string desc, string dirImg) {

            this.nombre = title;
            this.desc = desc;
            this.img = dirImg;
            this.generos = new List<String>();
            this.numSubCarpetas = 0;
            this.numArchivos = 0;
        }
        public CarpetaClass(string title, string desc) {

            this.nombre = title;
            this.desc = desc;
            this.img = "";
            this.generos = new List<String>();
            this.numSubCarpetas = 0;
            this.numArchivos = 0;
        }

        public void increaseSubCarpetas() {
            this.numSubCarpetas++;
        }

        public void decreaseSubCarpetas() {
            this.numSubCarpetas--;
        }

        public void increaseArchivos() {
            this.numArchivos++;
        }

        public void decraseArchivos() {
            this.numArchivos--;
        }

    }
}