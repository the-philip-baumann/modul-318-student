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
        List<GroupBox> navigationItems = new List<GroupBox>();
        public MainWindow() {
            InitializeComponent();
            transport = new Transport();
            navigationItems.Add(groupBoxConnection);
            navigationItems.Add(groupBoxStation);
            navigationItems.Add(groupBoxAbfahrtstafel);
            groupBoxConnection.Visibility = Visibility.Hidden;
            groupBoxStation.Visibility = Visibility.Visible;
            groupBoxAbfahrtstafel.Visibility = Visibility.Hidden;
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
                    listBoxStationResult.Items.Add(station.Name.ToString());
                }
            } else {
                new TransportException("Keine Ergebnisse gefunden", "Für Ihre Suchanfrage wurde kein Ergebnis gefunden");
            }
        }

        private void searchConnectionByStartAndDestinationStation (object sender, RoutedEventArgs e) {
            string start = comboBoxStart.Text;
            string destination = comboBoxZiel.Text;
            List<Connection> connections = transport.GetConnections(start, destination).ConnectionList;
            displayConnectionsOnResultGrid(connections);
        }

        private void displayConnectionsOnResultGrid (List<Connection> connections) {
            if (connections.Count != 0) {
                foreach (Connection connection in connections) {
                    Console.WriteLine(connection.From);
                    listBoxConnectionResult.Items.Add(connection.From.Station.Name + "  -->  " + connection.To.Station.Name.ToString() + "  :  " + connection.Duration);
                }
            }
        }

        private void navigate(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            foreach (GroupBox box in navigationItems) {
                if (box.Name.ToString() == "groupBox" + button.Content.ToString()) {
                    box.Visibility = Visibility.Visible;
                } else {
                    box.Visibility = Visibility.Hidden;
                }
            }
        }

        private void searchStationBoardByStationName(object sender, RoutedEventArgs e) {
            if (textBoxStationBoardSearchValue.Text.Length != 0) {
                StationBoardRoot stationBoardRoot = transport.GetStationBoard(textBoxStationBoardSearchValue.Text, "0");
                List<StationBoard> stationBoards = stationBoardRoot.Entries.ToList<StationBoard>();
                foreach (StationBoard station in stationBoards) {
                    Console.WriteLine(stationBoardRoot.Station.Name + station.Name + station.To.ToString() + station.Category.ToString());
                    listBoxAbfahrtsTafelResult.Items.Add(stationBoardRoot.Station.Name.ToString() + "  -->  " + station.To.ToString() + "  :  " + station.Name);
                }
            } else {
                new TransportException("Keine Ergebnisse gefunden", "Für Ihre Suchanfrage wurde kein Ergebnis gefunden");
            }
        }
    }
}
