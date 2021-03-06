﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ProyectoWPF.Reproductor;

namespace ProyectoWPF.Components
{
    /// <summary>
    /// Lógica de interacción para VideoElement.xaml
    /// </summary>
    public partial class VideoElement : UserControl
    {

        bool noImg = false;
        private VIGallery main;

        public VideoElement(VIGallery vi){
            InitializeComponent();
            main = vi;
        }

        private void reproduceButton_MouseEnter(object sender, EventArgs e) {
            if (!noImg) {
                gridMouseOver.Visibility = Visibility.Visible;
            }

        }

        private void reproduceButton_MouseLeave(object sender, EventArgs e) {
            if (!noImg) {
                gridMouseOver.Visibility = Visibility.Hidden;
            }
        }

        private void onClickReproducir(object sender, EventArgs e) {
            VI_Reproductor reproductor = main.getReproductor();
            reproductor.Visibility = Visibility.Visible;
            List<Uri> listaArchivos = new List<Uri>();
            List<long> listaId = new List<long>();
            List<string> listaNombre = new List<string>();
            /*if (_capitulo != null) {
                Uri u = new Uri(_capitulo.rutaWeb);
                listaArchivos.Add(u);
                listaId.Add(_capitulo.id);
                Temporada t = ListaOnline.getTempByCap(this._capitulo);
                Serie s = ListaOnline.getSerieByTemp(t);
                string nombre = s.nombre + " - Temporada " + t.numTemporada + " " + _capitulo.nombre;
                listaNombre.Add(nombre);
                reproductor.setListaNombres(listaNombre.ToArray());
                reproductor.setListaCapitulos(listaArchivos.ToArray(), listaId.ToArray()); ;
                reproductor.setVIGallery(main);
            } else if (_pelicula != null) {
                Uri u = new Uri(_pelicula.rutaWeb);
                listaId.Add(_pelicula.id);
                listaArchivos.Add(u);
                string nombre = _pelicula.nombre;
                listaNombre.Add(nombre);
                reproductor.setListaNombres(listaNombre.ToArray());
                reproductor.setListaPeliculas(listaArchivos.ToArray(), listaId.ToArray());
                reproductor.setVIGallery(main);
            }
            */
        }
        /*
        public void setCapitulo(Capitulo c) {
            this._capitulo = c;
            actualizar();
        }

        public void setPelicula(Pelicula p) {
            this._pelicula = p;
            actualizar();
        }

        public void actualizar() {
            setImg();
            if (this._capitulo != null) {
                Temporada t = ListaOnline.getTempByCap(this._capitulo);
                Serie s = ListaOnline.getSerieByTemp(t);
                setTitle(s.nombre + " Temporada " + t.numTemporada + " " + _capitulo.nombre);
            } else if (_pelicula != null) {
                setTitle(_pelicula.nombre);
            }
        }

        private void setImg() {
            if (this._capitulo != null) {
                try {
                    Temporada t = ListaOnline.getTempByCap(this._capitulo);
                    if (t != null) {
                        if (t.img.CompareTo("") != 0) {
                            BitmapImage bm = new BitmapImage(new Uri(t.img, UriKind.RelativeOrAbsolute));
                            ImageBrush ib = new ImageBrush(bm);
                            if (bm.Width > bm.Height) {
                                ib.Stretch = Stretch.UniformToFill;
                            }

                            bordeImg.Background = ib;
                            noImg = false;
                        }

                    } else if (ListaOnline.getSerieByTemp(t).img.CompareTo("") != 0) {
                        BitmapImage bm = new BitmapImage(new Uri(ListaOnline.getSerieByTemp(t).img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);
                        if (bm.Width > bm.Height) {
                            ib.Stretch = Stretch.UniformToFill;
                        }

                        bordeImg.Background = ib;
                        noImg = false;
                    } else {
                        gridMouseOver.Visibility = Visibility.Visible;
                        noImg = true;
                    }

                } catch (Exception e) {
                    gridMouseOver.Visibility = Visibility.Visible;
                    noImg = true;
                    Console.WriteLine(e.Message);
                }
            } else if (this._pelicula != null) {
                if (this._pelicula.img.CompareTo("") != 0) {
                    try {
                        BitmapImage bm = new BitmapImage(new Uri(_pelicula.img, UriKind.RelativeOrAbsolute));
                        ImageBrush ib = new ImageBrush(bm);
                        if (bm.Width > bm.Height) {
                            ib.Stretch = Stretch.UniformToFill;
                        }

                        bordeImg.Background = ib;
                        noImg = false;
                    } catch (Exception e) {
                        gridMouseOver.Visibility = Visibility.Visible;
                        noImg = true;
                        Console.WriteLine(e.Message);
                    }
                } else {
                    gridMouseOver.Visibility = Visibility.Visible;
                    noImg = true;
                }
            }
        }

        public void setTitle(string title) {
            videoTitle.Content = title;
        }
        */
    }
}
