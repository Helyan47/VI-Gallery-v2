﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ICollection<WrapPanelPrincipal> wrapsPrincipales;
        private ICollection<Button> botonesMenu;
        private Lista lista;
        string[] folders;
        private int num1, num2, num3 = 0;
        private int num = 0;
        private Carpeta aux;
        private SubCarpeta aux2;
        List<string> rutas = new List<string>();
        SaveData sv = new SaveData("ArchivoData.txt");
        private UIElementCollection botones;
        private Button activatedButton;
        public MainWindow() {
            InitializeComponent();
            botones = buttonStack.Children;
            botonesMenu = new List<Button>();
            wrapsPrincipales = new List<WrapPanelPrincipal>();
            lista = new Lista();
            bool check=loadData();
            if (check == true) {

            } else {

                Button b = new Button();
                b.Height = 100;
                b.FontSize = 40;
                b.BorderThickness = new System.Windows.Thickness(0);
                b.FontWeight = FontWeights.Bold;
                b.Foreground = Brushes.White;
                b.Visibility = Visibility.Visible;
                b.Content = "Serie";
                b.Name = "Serie";
                b.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                b.Click += onClickButtonMenu;

                botonesMenu.Add(b);
                buttonStack.Children.Add(b);
                int cont = 0;
                botonesMenu.Add(b);
                string name = b.Content.ToString();
                aux = new Carpeta(this);
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                wp.Visibility = Visibility.Visible;
                activatedButton = b;
                wrapsPrincipales.Add(wp);
                lista = new Lista(wrapsPrincipales, botonesMenu);
            }
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;

            if (lista.buttonInButtons(b)) {
                lista.hideAll();
                
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                lista.showWrapFromButton(b);

            }
            foreach(Button h in botones) {
                h.ClearValue(Button.BackgroundProperty);
            }
            activatedButton = b;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
        }


        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            string[] files = new string[0];
            using (var folderDialog = new CommonOpenFileDialog()) {

                folderDialog.IsFolderPicker = true;
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                    folders = Directory.GetDirectories(folderDialog.FileName);

                    for (int i = 0; i < folders.Length; i++) {
                        rutas.Add(folders[i]);

                        string[] aux = Directory.GetDirectories(folders[i]);
                        for (int j = 0; j < aux.Length; j++) {
                            rutas.Add(aux[j]);
                        }
                    }
                    if (folders != null) {
                        addText(folders);
                    }


                }
            }
            lista.hideAllExceptPrinc();
            
        }

        private void addSubCarpeta() {
            SubCarpeta c = new SubCarpeta();
            c.setTitle("");
            NewSubCarpeta n = new NewSubCarpeta();
           
            n.setSubCarpeta(c);
            n.ShowDialog();
            if (n.getSubCarpeta().getTitle() != "") {

                WrapPanelPrincipal p = lista.getSubWrapsVisibles();
                lista.addSubCarpeta(c);
                if (p != null) {
                    if (p.getCarpeta() == null) {
                        c.setDatos(p.getSubCarpeta().getSerie(), p,
                        p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());

                        string name = activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());
                        //sv.saveData(c.getRutaPrograma(), name);


                    } else {

                        c.setDatos(p.getCarpeta().getSerie(), p,
                        p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());

                        string name = activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());
                        //sv.saveData(c.getRutaPrograma(), name);

                    }

                    sv.saveSubFolder(c);

                }



                c.actualizar();
                c.changeMode(lista.actualiceMode(activatedButton));
                c.Visibility = Visibility.Visible;
            } else {
                //c.Controls.Remove(c);
            }
        }

        private SubCarpeta addSubCarpetaCompleta(Carpeta p1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            lista.addSubCarpeta(c);
            if (p1.getListaCarpetas() == null) {
            }
            p1.clickEspecial();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = p1.GetWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));
                    
                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    sv.saveSubFolder(c);
                    //sv.saveData(c.getRutaPrograma(), name);

                    c.changeMode(lista.actualiceMode(activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    sv.saveSubFolder(c);
                    //sv.saveData(c.getRutaPrograma(), name);

                    c.changeMode(lista.actualiceMode(activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }

        private SubCarpeta addSubCarpetaCompleta(SubCarpeta sp1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            lista.addSubCarpeta(c);
            if (sp1.GetListaCarpetas() == null) {
                System.Windows.MessageBox.Show("es null2");
            }
            sp1.click();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = sp1.getWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getSubCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(nombre);
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    sv.saveSubFolder(c);
                    //sv.saveData(c.getRutaPrograma(), name);

                    c.changeMode(lista.actualiceMode(activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    //c.setIdHijo(p.getCarpeta().getNumSubCarp());
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    c.setRutaDirectorio(p.getCarpeta().getRutaDirectorio()+"/"+ c.getTitle());
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma()+"/"+c.getTitle());

                    sv.saveSubFolder(c);
                    //sv.saveData(c.getRutaPrograma(), name);

                    c.changeMode(lista.actualiceMode(activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }


        private void addText(string[] files) {

            for (int i = 0; i < files.Length; i++) {
                /*textBox1.Text += files[i] + "\r\n";*/
                if (files == folders) {
                    aux = addCarpetaCompleta(files[i]);

                } else {

                    aux2 = lista.searchRuta(Directory.GetParent(files[i]).FullName);
                    if (!checkString(files[i])) {
                        aux2 = addSubCarpetaCompleta(aux2, files[i]);
                    } else {
                        aux2 = addSubCarpetaCompleta(aux, files[i]);


                    }


                }
                if (Directory.GetDirectories(files[i]) != null) {
                    addText(Directory.GetDirectories(files[i]));
                }
                //if (Directory.GetFiles(files[i]) != null) {
                //    string[] archivos = Directory.GetFiles(files[i]);
                //    for (int j = 0; j < archivos.Length; j++) {
                //        /*textBox1.Text += archivos[i] + "\r\n";*/

                //    }

                //}
            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            addCarpeta();
        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this.lista);
            lista.addCarpeta(p1);
            AddCarpeta newSerie = new AddCarpeta(p1);
            newSerie.ShowDialog();
            if (!p1.getSerie().getTitle().Equals("")) {
                WrapPanelPrincipal aux = lista.getWrapVisible();

                p1.actualizar();

                string name = activatedButton.Name;
                p1.getSerie().setTipo(name);
                p1.setRutaPrograma("C/"+name+"/" + p1.getSerie().getTitle());

                sv.saveFolder(p1);
                //sv.saveData(p1.getRutaPrograma(),name);

                p1.SetGridPadre(gridPrincipal);
                aux.addCarpeta(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

                p1.changeMode(lista.actualiceMode(activatedButton));

            } else {
                //p1.Controls.Remove(p1);
            }

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this.lista);
            p1.setRutaDirectorio(filename);
            lista.addCarpeta(p1);

            WrapPanelPrincipal aux = lista.getWrapVisible();

            

            SerieClass s = new SerieClass(System.IO.Path.GetFileNameWithoutExtension(filename), "");
            p1.setSerie(s);
            p1.actualizar();

            string name = activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma("C/" + name + "/" + p1.getSerie().getTitle());

            sv.saveFolder(p1);
            


            aux.addCarpeta(p1);
            
            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.changeMode(lista.actualiceMode(activatedButton));

            return p1;
        }

        private Carpeta loadCarpeta(SaveCarpeta sc) {
            Carpeta p1 = new Carpeta(this);
            p1.setListaCarpetas(this.lista);
            lista.addCarpeta(p1);

            WrapPanelPrincipal aux = lista.getWrapVisible();

            SerieClass s = new SerieClass(sc.getName(),sc.getDesc());
            p1.setSerie(s);
            p1.getSerie().setDirImge(sc.getDirImg());
            p1.actualizar();

            string name = activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma(sc.getRutaPrograma());




            aux.addCarpeta(p1);

            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.changeMode(lista.actualiceMode(activatedButton));

            p1.clickEspecial();

            return p1;
        }

        private SubCarpeta loadSubCarpeta(SaveCarpeta sc) {
            SubCarpeta c = new SubCarpeta();
            lista.addSubCarpeta(c);
            c.setRutaPrograma(sc.getRutaPrograma());
            object obj = lista.getFolderRuta(sc.getRutaPrograma());
            WrapPanelPrincipal p=null;
            if (typeof(Carpeta).IsInstanceOfType(obj)) {
                Carpeta aux = (Carpeta)obj;
                p = aux.GetWrapCarpPrincipal();
            } else if (typeof(SubCarpeta).IsInstanceOfType(obj)) {
                SubCarpeta aux = (SubCarpeta)obj;
                p = aux.getWrapCarpPrincipal();
            }
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
             
            if (p != null) {
                if (p.getCarpeta() == null) {
                    c.setDatos(p.getSubCarpeta().getSerie(), p,
                    p.getSubCarpeta().GetListaCarpetas(), p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);
                    


                    c.changeMode(lista.actualiceMode(activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p,
                    p.getCarpeta().getListaCarpetas(), p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = activatedButton.Name;
                    c.getSerie().setTipo(name);

                    c.changeMode(lista.actualiceMode(activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
            return c;
        }

        private void Return_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            SubCarpeta p = lista.getSubWrapsVisibles().getSubCarpeta();
            Carpeta p1 = lista.getSubWrapsVisibles().getCarpeta();
            if (p != null) {
                p.clickInverso();
            } else if(p1!= null){
                p1.clickInverso();
            } else {
                MessageBox.Show("null");
            }
            
        }

        public bool loadData() {
            bool check;
            if (File.Exists("ArchivoData.txt")) {
                check = true;
                ICollection<SaveCarpeta> folders = sv.loadFolders();
                ICollection<Button> ib = sv.loadButtons(folders);
                ICollection<SaveCarpeta> subFolders = sv.loadSubFolders();
                int cont = 0;
                foreach (Button b in ib) {
                    b.Click += onClickButtonMenu;
                    
                    botonesMenu.Add(b);
                    buttonStack.Children.Add(b);
                    
                    string name = b.Content.ToString();
                    aux = new Carpeta(this);
                    WrapPanelPrincipal wp = new WrapPanelPrincipal();
                    wp.Name = name;
                    gridPrincipal.Children.Add(wp);
                    
                    if (cont == 0) {
                        wp.Visibility = Visibility.Visible;
                        activatedButton = b;
                        b.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    } else {
                        wp.Visibility = Visibility.Hidden;
                    }
                    cont++;

                    wrapsPrincipales.Add(wp);
                }
                lista = new Lista(wrapsPrincipales, botonesMenu);

                foreach (SaveCarpeta sc in folders){
                    loadCarpeta(sc);
                }

                foreach(SaveCarpeta sc in subFolders) {
                    loadSubCarpeta(sc);
                }

                
                return check;
            } else {
                check = false;
                return check;
            }

        }

        public bool checkString(string s) {
            foreach (string h in rutas) {
                if (s.Equals(h)) {
                    //MessageBox.Show(s);
                    return true;
                }
            }
            return false;
        }

        public void ReturnVisibility(bool flag) {
            if (flag) {
                Return.Visibility = Visibility.Visible;
            } else {
                Return.Visibility = Visibility.Hidden;
            }
        }

        public void CerrarApp(object sender, RoutedEventArgs e) {
            this.Close();
        }
        public void MaximizeApp(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
            }else if (this.WindowState == WindowState.Maximized) {
                this.WindowState = WindowState.Normal;
                this.Width = 1000;
                this.Height = 700;
            }
            
        }

        public void MinimizeApp(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        public void ChangeMode(object sender,RoutedEventArgs e) {
            lista.changeMode(activatedButton);
        }
    }
}