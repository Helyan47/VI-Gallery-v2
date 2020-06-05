﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleccionarProfile.Data {
    public class ArchivoClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaSistema { get; set; }
        public string rutaPrograma { get; set; }
        public string img { get; set; }
        public long idCarpeta { get; set; }

        public ArchivoClass(long id, string nombre, string rutaSistema, string rutaPrograma, string img, long idCarpeta) {
            this.id = id;
            this.nombre = nombre;
            this.rutaSistema = rutaSistema;
            this.rutaPrograma = rutaPrograma;
            this.img = img;
            this.idCarpeta = idCarpeta;
        }

        public ArchivoClass(string nombre, string rutaSistema, string rutaPrograma, string img, long idCarpeta) {
            this.nombre = nombre;
            this.rutaSistema = rutaSistema;
            this.rutaPrograma = rutaPrograma;
            this.img = img;
            this.idCarpeta = idCarpeta;
        }
    }
}
