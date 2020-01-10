﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using ProyectoWPF.NewFolders;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para VIGallery.xaml
    /// </summary>
    public partial class VIGallery : Window {

        private ICollection<WrapPanelPrincipal> _wrapsPrincipales;
        private ICollection<Button> _botonesMenu;
        string[] _folders;
        private Carpeta _aux;
        private SubCarpeta _aux2;
        List<string> _rutas = new List<string>();
        SaveData _saveData = new SaveData();
        private UIElementCollection _botones;
        private Button _activatedButton;
        public int firstFolder = 0;
        public static string userNick = "Helyan";
        public static int idUsuario = 1;
        //Establece si se ha iniciado con conexion o sin conexion
        public static bool conexionMode = false;
        public static string profile = "Profile1";
        public VIGallery() {
            InitializeComponent();
            bool conexion = false;
            conexionMode = conexion;
            _botones = buttonStack.Children;
            _botonesMenu = new List<Button>();
            _wrapsPrincipales = new List<WrapPanelPrincipal>();
            if (!conexionMode) {
                bool check = loadData();
                if (check != true) {

                }
            }
            
        }


        public void onClickButtonMenu(object sender,EventArgs e) {
            Button b = (Button)sender;

            if (Lista.buttonInButtons(b)) {
                Lista.hideAll();
                GridSecundario.SetValue(Grid.RowProperty, 1);
                GridPrincipal.SetValue(Grid.RowProperty, 0);
                Lista.showWrapFromButton(b);

            }
            foreach(Button h in _botones) {
                h.ClearValue(Button.BackgroundProperty);
            }
            _activatedButton = b;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF595959"));
            Return.Visibility = Visibility.Hidden;
        }


        private void NewTemp_Click(object sender, EventArgs e) {
            addSubCarpeta();
        }

        private void Button_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            if (_activatedButton != null) {
                string[] files = new string[0];
                using (var folderDialog = new CommonOpenFileDialog()) {

                    folderDialog.IsFolderPicker = true;
                    firstFolder = 0;
                    if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok && !string.IsNullOrWhiteSpace(folderDialog.FileName)) {
                        _folders = Directory.GetDirectories(folderDialog.FileName);

                        for (int i = 0; i < _folders.Length; i++) {
                            _rutas.Add(_folders[i]);

                            string[] aux = Directory.GetDirectories(_folders[i]);
                            for (int j = 0; j < aux.Length; j++) {
                                _rutas.Add(aux[j]);
                            }
                        }
                        if (_folders != null) {
                            addText(_folders);
                        }


                    }
                }
                Lista.hideAllExceptPrinc();
            } else {
                menuButtons.BorderThickness = new Thickness(5);
                MessageBox.Show("No has creado ninguno menú");
                menuButtons.BorderThickness = new Thickness(0);
            }

}

        private void addSubCarpeta() {
            SubCarpeta c = new SubCarpeta();
            c.setTitle("");
            WrapPanelPrincipal p = Lista.getSubWrapsVisibles();
            NewSubCarpeta n = null;
            if (p.getCarpeta() == null) {
                n = new NewSubCarpeta(p.getSubCarpeta().getRutaPrograma());
            } else {
                n = new NewSubCarpeta(p.getCarpeta().getRutaPrograma());
            }
            
           
            n.setSubCarpeta(c);
            n.ShowDialog();
            if (n.getSubCarpeta().getTitle() != "") {

                
                Lista.addSubCarpeta(c);
                if (p != null) {
                    if (p.getCarpeta() == null) {
                        c.setDatos(p.getSubCarpeta().getSerie(), p, p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());


                    } else {

                        c.setDatos(p.getCarpeta().getSerie(), p, p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    }

                    if (conexionMode) {
                        Conexion.uploadSubFolder(c);
                    } else {
                        _saveData.saveSubFolder(c);
                    }

                    c.actualizar();
                    c.changeMode(Lista.actualiceMode(_activatedButton));
                    c.Visibility = Visibility.Visible;

                }



                
            } else {
                c = null;
            }
        }

        private SubCarpeta addSubCarpetaCompleta(Carpeta p1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            
            p1.clickEspecial();
            //FlowCarpeta p = listaSeries.getFlowCarpVisible();
            WrapPanelPrincipal p = p1.GetWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());
                    bool checkIfExists = Lista.Contains(c.getRutaPrograma());

                    if (!checkIfExists) {
                        Lista.addSubCarpeta(c);

                        c.setDatos(p.getSubCarpeta().getSerie(), p, p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                        c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaDirectorio(nombre);

                        if (conexionMode) {
                            Conexion.uploadSubFolder(c);
                        } else {
                            _saveData.saveSubFolder(c);
                        }

                        c.changeMode(Lista.actualiceMode(_activatedButton));
                        c.actualizar();
                        c.Visibility = Visibility.Visible;
                    } else {
                        c = null;
                    }

                        

                } else {
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma() + "/" + c.getTitle());

                    bool checkIfExists = Lista.Contains(c.getRutaPrograma());

                    if (!checkIfExists) {
                        Lista.addSubCarpeta(c);

                        c.setDatos(p.getCarpeta().getSerie(), p, p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                        c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaDirectorio(nombre);

                        if (conexionMode) {
                            Conexion.uploadSubFolder(c);
                        } else {
                            _saveData.saveSubFolder(c);
                        }

                        

                        c.changeMode(Lista.actualiceMode(_activatedButton));
                        c.actualizar();
                        c.Visibility = Visibility.Visible;
                    } else {
                        c = null;
                    }

                }

            }


            
            return c;
        }

        private SubCarpeta addSubCarpetaCompleta(SubCarpeta sp1, string nombre) {
            SubCarpeta c = new SubCarpeta();
            
            sp1.click();
            WrapPanelPrincipal p = sp1.getWrapCarpPrincipal();
            c.setRutaDirectorio(nombre);
            if (p != null) {
                if (p.getCarpeta() == null) {
                    
                    c.setRutaPrograma(p.getSubCarpeta().getRutaPrograma() + "/" + c.getTitle());
                    bool checkIfExists = Lista.Contains(c.getRutaPrograma());
                    if (!checkIfExists) {
                        Lista.addSubCarpeta(c);
                        c.setDatos(p.getSubCarpeta().getSerie(), p, p.getSubCarpeta().GetGridCarpeta());
                        p.addSubCarpeta(c);
                        p.getSubCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                        c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaDirectorio(nombre);

                        if (conexionMode) {
                            Conexion.uploadSubFolder(c);
                        } else {
                            _saveData.saveSubFolder(c);
                        }

                        c.changeMode(Lista.actualiceMode(_activatedButton));
                        c.actualizar();
                        c.Visibility = Visibility.Visible;
                    } else {
                        c = null;
                    }

                        

                } else {
                    c.setRutaPrograma(p.getCarpeta().getRutaPrograma()+"/"+c.getTitle());

                    bool checkIfExists = Lista.Contains(c.getRutaPrograma());
                    if (!checkIfExists) {
                        Lista.addSubCarpeta(c);
                        c.setDatos(p.getCarpeta().getSerie(), p, p.GetGridSubCarpetas());
                        p.addSubCarpeta(c);
                        p.getCarpeta().AddSubCarpetas();
                        c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                        c.setTitle(System.IO.Path.GetFileNameWithoutExtension(nombre));

                        string name = _activatedButton.Name;
                        c.getSerie().setTipo(name);
                        c.setRutaDirectorio(p.getCarpeta().getRutaDirectorio() + "/" + c.getTitle());

                        if (conexionMode) {
                            Conexion.uploadSubFolder(c);
                        } else {
                            _saveData.saveSubFolder(c);
                        }

                        c.changeMode(Lista.actualiceMode(_activatedButton));
                        c.actualizar();
                        c.Visibility = Visibility.Visible;

                    } else {
                        c = null;
                    }

                        

                }

            }
            
            return c;
        }


        private void addText(string[] files) {

            for (int i = 0; i < files.Length; i++) {
                if (files == _folders) {
                    _aux = addCarpetaCompleta(files[i]);
                    if (_aux == null) {
                        if (i != files.Length - 1) {
                            i++;
                        }
                        
                    }
                } else {

                    _aux2 = Lista.searchRuta(Directory.GetParent(files[i]).FullName);
                    if (!checkString(files[i])) {
                        _aux2 = addSubCarpetaCompleta(_aux2, files[i]);
                    } else {
                        if (_aux != null) {
                            _aux2 = addSubCarpetaCompleta(_aux, files[i]);
                        }
                    }
                }
                if (Directory.GetDirectories(files[i]) != null) {
                    addText(Directory.GetDirectories(files[i]));
                }
            }
        }

        private void Button_AddCarpeta(object sender, EventArgs e) {
            if (_activatedButton != null) {
                addCarpeta();
            } else {
                menuButtons.BorderThickness = new Thickness(5);
                MessageBox.Show("No has creado ninguno menú");
                menuButtons.BorderThickness = new Thickness(0);
            }
        }

        private void addCarpeta() {
            Carpeta p1 = new Carpeta(this);
            
            AddCarpeta newSerie = new AddCarpeta(p1, _activatedButton);
            newSerie.ShowDialog();

            if (newSerie.createdSerie()) {
                Lista.addCarpeta(p1);
                WrapPanelPrincipal aux = Lista.getWrapVisible();

                p1.actualizar();

                string name = _activatedButton.Name;
                p1.getSerie().setTipo(name);
                p1.setRutaPrograma(profile + "|C/" + name + "/" + p1.getSerie().getTitle());

                if (conexionMode) {
                    Conexion.uploadSerie(p1.getSerie());
                    Conexion.uploadFolder(p1);
                } else {
                    _saveData.saveFolder(p1);
                }

                p1.SetGridPadre(gridPrincipal);
                aux.addCarpeta(p1);
                p1.setPadreSerie(aux);
                p1.SetGridsOpciones(GridPrincipal, GridSecundario);

                p1.changeMode(Lista.actualiceMode(_activatedButton));
            } else {
                p1 = null;
            }
            

        }

        private Carpeta addCarpetaCompleta(string filename) {
            Carpeta p1 = new Carpeta(this);
            p1.setRutaDirectorio(filename);
            

            WrapPanelPrincipal aux = Lista.getWrapVisible();

            SerieClass s = new SerieClass(System.IO.Path.GetFileNameWithoutExtension(filename), "");
            s.setRuta("Serie/" + _activatedButton.Content + "/" + s.getTitle());
            p1.setSerie(s);
            p1.actualizar();

            string name = _activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma(profile+"|C/" + name + "/" + p1.getSerie().getTitle());
            bool checkIfExists = Lista.Contains(p1.getRutaPrograma());
            if (!checkIfExists) {
                Lista.addCarpeta(p1);

                string[] files = System.IO.Directory.GetFiles(filename, "cover.*");
                if (files.Length > 0) {
                    p1.getSerie().setDirImg(files[0]);
                }


                if (conexionMode) {
                    Conexion.uploadSerie(p1.getSerie());
                    Conexion.uploadFolder(p1);
                } else {
                    _saveData.saveFolder(p1);
                }

                aux.addCarpeta(p1);

                p1.SetGridsOpciones(GridPrincipal, GridSecundario);
                p1.setPadreSerie(aux);
                p1.SetGridPadre(gridPrincipal);

                p1.changeMode(Lista.actualiceMode(_activatedButton));
            } else {
                p1 = null;
                s = null;
            }
            return p1;
        }

        private void loadCarpeta(SaveDataType sc) {
            Carpeta p1 = new Carpeta(this);
            Lista.addCarpeta(p1);

            WrapPanelPrincipal aux = Lista.getWrapVisible();

            SerieClass s = new SerieClass(sc.getName(),sc.getDesc());
            p1.setSerie(s);
            p1.getSerie().setDirImg(sc.getDirImg());
            p1.actualizar();

            string name = _activatedButton.Name;
            p1.getSerie().setTipo(name);
            p1.setRutaPrograma(sc.getRutaPrograma());

            aux.addCarpeta(p1);

            p1.SetGridsOpciones(GridPrincipal, GridSecundario);
            p1.setPadreSerie(aux);
            p1.SetGridPadre(gridPrincipal);

            p1.changeMode(Lista.actualiceMode(_activatedButton));

            p1.clickEspecial();
        }

        private void loadSubCarpeta(SaveDataType sc) {
            SubCarpeta c = new SubCarpeta();
            Lista.addSubCarpeta(c);
            c.setRutaPrograma(sc.getRutaPrograma());
            object obj = Lista.getFolderRuta(sc.getRutaPrograma());
            WrapPanelPrincipal p=null;
            if (obj is Carpeta) {
                Carpeta aux = (Carpeta)obj;
                p = aux.GetWrapCarpPrincipal();
            } else if (obj is SubCarpeta) {
                SubCarpeta aux = (SubCarpeta)obj;
                p = aux.getWrapCarpPrincipal();
            }
             
            if (p != null) {
                if (p.getCarpeta() == null) {

                    c.setDatos(p.getSubCarpeta().getSerie(), p, p.getSubCarpeta().GetGridCarpeta());
                    p.addSubCarpeta(c);
                    p.getSubCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getSubCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);

                    c.changeMode(Lista.actualiceMode(_activatedButton));

                } else {

                    c.setDatos(p.getCarpeta().getSerie(), p, p.GetGridSubCarpetas());
                    p.addSubCarpeta(c);
                    p.getCarpeta().AddSubCarpetas();
                    c.setMenuCarpeta(p.getCarpeta().GetMenuCarpeta());
                    c.setTitle(sc.getName());

                    string name = _activatedButton.Name;
                    c.getSerie().setTipo(name);

                    c.changeMode(Lista.actualiceMode(_activatedButton));

                }

            }


            c.actualizar();


            c.Visibility = Visibility.Visible;
        }

        private void Return_MouseLeftButtonUp(object sender, RoutedEventArgs e) {
            SubCarpeta p = Lista.getSubWrapsVisibles().getSubCarpeta();
            Carpeta p1 = Lista.getSubWrapsVisibles().getCarpeta();
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
                ICollection<SaveDataType> folders = _saveData.loadFolders();
                ICollection<Button> ib = _saveData.loadButtons(folders);
                ICollection<SaveDataType> subFolders = _saveData.loadSubFolders();
                int cont = 0;
                foreach (Button b in ib) {
                    b.Click += onClickButtonMenu;
                    
                    _botonesMenu.Add(b);
                    Lista.addButton(b);
                    buttonStack.Children.Add(b);
                    
                    string name = b.Content.ToString();
                    _aux = new Carpeta(this);
                    WrapPanelPrincipal wp = new WrapPanelPrincipal();
                    wp.Name = name;
                    gridPrincipal.Children.Add(wp);
                    
                    if (cont == 0) {
                        wp.Visibility = Visibility.Visible;
                        _activatedButton = b;
                        b.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    } else {
                        wp.Visibility = Visibility.Hidden;
                    }
                    cont++;
                    Lista.addWrapPrincipal(wp);
                    _wrapsPrincipales.Add(wp);
                }

                foreach (SaveDataType sc in folders){
                    Lista.getButtonFromFolder(sc.getRutaPrograma()).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    loadCarpeta(sc);
                }

                foreach(SaveDataType sc in subFolders) {
                    Lista.getButtonFromFolder(sc.getRutaPrograma()).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    loadSubCarpeta(sc);
                }

                if (Lista.getFirstButton() != null) {
                    Lista.getFirstButton().RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                
                return check;
            } else {
                check = false;
                return check;
            }

        }

        private void addButtonClick(object sender,EventArgs e) {
            Button newButton = new Button();
            newButton.Content = "";
            AddButton a=new AddButton(newButton);
            a.ShowDialog();
            if (newButton.Content.ToString().CompareTo("") != 0) {
                newButton.Height = 100;
                newButton.FontSize = 40;
                newButton.BorderThickness = new System.Windows.Thickness(0);
                newButton.FontWeight = FontWeights.Bold;
                newButton.Foreground = Brushes.White;
                newButton.Visibility = Visibility.Visible;
                newButton.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                newButton.Click += onClickButtonMenu;
                

                _botonesMenu.Add(newButton);
                Lista.addButton(newButton);
                buttonStack.Children.Add(newButton);
                int cont = 0;
                string name = newButton.Content.ToString();
                _aux = new Carpeta(this);
                WrapPanelPrincipal wp = new WrapPanelPrincipal();
                wp.Name = name;
                gridPrincipal.Children.Add(wp);
                wp.Visibility = Visibility.Visible;
                _activatedButton = newButton;
                _wrapsPrincipales.Add(wp);
                
                Lista.addWrapPrincipal(wp);

                onClickButtonMenu(newButton, e);

            }
            
        }

        private void removeButtonClick(object sender, EventArgs e) {

        }

        public bool checkString(string s) {
            foreach (string h in _rutas) {
                if (s.Equals(h)) {
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
            Lista.changeMode(_activatedButton);
        }
    }
}