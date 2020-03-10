using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SwissTransport;

namespace TransportGUI {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Transport transport;
        public MainWindow() {
            InitializeComponent();
            transport = new Transport();
            
        }

        private void searchStationByName (object sender, RoutedEventArgs e) {
            if (textBoxSearchValue.Text.Length != 0) {
                // Call webservice with the users query
                string query = textBoxSearchValue.Text;
                Stations station = transport.GetStations(query);
                displayStationsOnListBox(station);
            } else {
                new TransportException("Textbox ist leer", "Geben sie eine Station an!");
            }
        }

        private void displayStationsOnListBox (Stations stations) {
            List<Station> listStations = stations.StationList;
            // Check if response contains a result and is not empty
            if (listStations.Count != 0) {
                foreach (Station station in listStations) {
                    listBoxResult.Items.Add(station.Name.ToString());
                }
            } else {
                new TransportException("Keine Ergebnisse gefunden", "Für Ihre Suchanfrage wurde kein Ergebnis gefunden");
            }
        }
       
    }
}
