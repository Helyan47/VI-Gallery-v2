﻿using ProyectoWPF.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProyectoWPF {
    public class SaveData {

        public static string _archivoData= "../../../ProyectoWPF/bin/Debug/ArchivoData.txt";

        public SaveData() {

        }

        public void saveData(string s,string name) {
            StreamWriter sw = new StreamWriter(_archivoData, true);
            sw.WriteLine(s);
            sw.Close();
        }

        public void saveFolder(Carpeta c) {
            SaveDataType carpetaData = new SaveDataType(c.getSerie().getTitle(), true, c.getDescripcion(), c.getRutaPrograma(), c.getSerie().getTipo(),c.getSerie().getDirImg(), c._profile);
            IFormatter formatter=new BinaryFormatter();
            using (FileStream stream = new FileStream(_archivoData, FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }
        }

        public void saveSubFolder(SubCarpeta c) {
            SaveDataType carpetaData = new SaveDataType(c.getTitle(), false, true, c.getRutaPrograma(), c.getSerie().getTipo(),c.getDirImg(),c._profile);
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_archivoData, FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }
        }

        public ICollection<SaveDataType> loadFolders() {
            ICollection<SaveDataType> ic = new List<SaveDataType>();
            ICollection<SaveDataType> objects = new List<SaveDataType>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(_archivoData)) {
                using (FileStream stream = new FileStream(_archivoData, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveDataType aux = (SaveDataType)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveDataType c in objects) {
                    if (c.getProfile().CompareTo(VIGallery._profile) == 0) {
                        if (c.isCarpeta()) {
                            if (!c.isSubCarpeta()) {
                                ic.Add(c);
                            }
                        }
                    }
                    
                }
            }
            

            return ic;
        }

        public ICollection<SaveDataType> loadSubFolders() {
            
            if (File.Exists(_archivoData)) {
                ICollection<SaveDataType> ic = new List<SaveDataType>();
                ICollection<SaveDataType> objects = new List<SaveDataType>();
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_archivoData, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveDataType aux = (SaveDataType)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveDataType c in objects) {
                    if (c.getProfile().CompareTo(VIGallery._profile) == 0) {
                        if (c.isSubCarpeta()) {
                            ic.Add(c);
                        }
                    }

                }
                return ic;
            }

            return null;
        }

        public ICollection<Button> loadButtons(ICollection<SaveDataType> ic) {
            ICollection<Button> botones = new List<Button>();
            ICollection<string> items = new List<string>();

            foreach (SaveDataType c in ic) {
                if (c.getProfile().CompareTo(VIGallery._profile) == 0) {
                    if (c.isCarpeta()) {
                        if (!c.isSubCarpeta()) {
                            string name = getName(c.getRutaPrograma());
                            bool exist = false;
                            foreach (string s in items) {
                                if (s.Equals(name)) {
                                    exist = true;
                                }
                            }
                            if (!exist) {
                                items.Add(name);
                            }
                        }
                    }
                }

            }
            foreach(string s in items) {
                Button b = new Button();
                b.Height = 100;
                b.FontSize = 40;
                b.BorderThickness = new System.Windows.Thickness(0);
                b.FontWeight = FontWeights.Bold;
                b.Foreground = Brushes.White;
                b.Visibility = Visibility.Visible;
                b.Content = s;
                b.Name = s;
                b.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                botones.Add(b);
            }

            return botones;
        }

        public string getName(string ruta) {
            string[] splitted = ruta.Split('/');

            return splitted[1];
        }

        public static List<string> getProfiles() {
            if (File.Exists(_archivoData)) {
                List<string> lista = new List<string>();
                ICollection<SaveDataType> objects = new List<SaveDataType>();
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_archivoData, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveDataType aux = (SaveDataType)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                    foreach (SaveDataType c in objects) {
                        if (!lista.Contains(c.getProfile())) {
                            lista.Add(c.getProfile());
                        }

                    }
                }
                return lista;
            } else {
                MessageBox.Show("No se ha encontrado el archivo");
            }

            return null;
        }

        public static bool profileExists(string s) {
            if (getProfiles().Contains(s)) {
                return true;
            }
            return false;
        }
    }
}
