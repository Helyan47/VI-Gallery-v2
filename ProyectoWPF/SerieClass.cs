﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF {
    public class SerieClass {
        private int idSerie;
        private String title;
        private int numCapitulos;
        private String desc;
        private String tipo;
        private String dirImg;
        private ICollection<String> generos;

        public SerieClass() {

            this.idSerie = 0;
            this.title = "";
            this.numCapitulos = 0;
            this.desc = "";
            this.tipo = "";
            this.dirImg = "";
            this.generos = new List<String>();
        }

        public SerieClass(string title, string desc, string dirImg, ICollection<string> generos) {

            this.title = title;
            this.desc = desc;
            this.dirImg = dirImg;
            this.generos = generos;
            this.numCapitulos = 0;
        }
        public SerieClass(string title, string desc, string dirImg) {

            this.title = title;
            this.desc = desc;
            this.dirImg = dirImg;
            this.generos = new List<String>();
            this.numCapitulos = 0;
        }
        public SerieClass(string title, string desc) {

            this.title = title;
            this.desc = desc;
            this.dirImg = "";
            this.generos = new List<String>();
            this.numCapitulos = 0;
        }


        public void setIdSerie(int serie) {

            this.idSerie = serie;
        }

        public int getIdSerie() {

            return this.idSerie;
        }


        public void setTitle(String titulo) {

            this.title = titulo;
        }

        public String getTitle() {

            return this.title;
        }

        public void setNumCapitulos(int num) {

            this.numCapitulos = num;
        }

        public int getNumCapitulos() {

            return this.numCapitulos;
        }

        public void setDesc(String desc) {

            this.desc = desc;
        }

        public String getDesc() {

            return this.desc;
        }

        public void setTipo(String tipo) {

            this.tipo = tipo;
        }

        public String getTipo() {

            return this.tipo;
        }

        public void setDirImge(String dir) {

            this.dirImg = dir;
        }

        public String getDirImg() {

            return this.dirImg;
        }


        public void addGenero(String genero) {

            generos.Add(genero);
        }

        public void removeGenero(String genero) {

            generos.Remove(genero);
        }

        public void setGeneros(ICollection<String> newGeneros) {

            generos = newGeneros;
        }

        public ICollection<String> getGeneros() {

            return generos;
        }
    }
}