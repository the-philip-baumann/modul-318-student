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
using System.Diagnostics;

namespace TransportGUI {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Transport transport;
        ApiSearches apiSearches;
        Navigation navigation;
        List<GroupBox> navigationItems = new List<GroupBox>();
        string yearMonthDay;
        private DateTime date;
        public MainWindow() {
            InitializeComponent();
            setupClasses();
            setupNavigation();
            setTimeInComboBox();
            disableButtons();
        }

        private void setupClasses () {
            // instantiate Classes
            transport = new Transport();
            apiSearches = new ApiSearches(transport);
            navigation = new Navigation();
        }

        private void setupNavigation () {
            // setup GUI visibility and add Groupboxes to list
            navigationItems.Add(groupBoxConnection);
            navigationItems.Add(groupBoxStation);
            navigationItems.Add(groupBoxAbfahrtstafel);
            groupBoxConnection.Visibility = Visibility.Hidden;
            groupBoxStation.Visibility = Visibility.Visible;
            groupBoxAbfahrtstafel.Visibility = Visibility.Hidden;
        }

        private void disableButtons () {
            buttonSearch.IsEnabled = false;
            buttonGoogleMaps.IsEnabled = false;
        }

        // Click event
        private void searchStationByName(object sender, RoutedEventArgs e) {
            Stations stations = apiSearches.searchForStation(comboBoxStationSearchValue.Text);
            // If validation failed
            if (stations != null) {
                displayStationsOnListBox(stations);
            }
        }

        private void displayStationsOnListBox (Stations stations) {
            List<Station> listStations = stations.StationList;
            // Check if response contains a result and is not empty
            if (listStations.Count != 0) {
                listBoxStationResult.Items.Clear();
                foreach (Station station in listStations) {
                    // Companys, Monuments and other places do not posess an id
                    if (station.Id != null) {
                        listBoxStationResult.Items.Add(station.Name.ToString());
                    }
                }
            } else {
                // Feedback
                new TransportException("Keine Ergebnisse gefunden", "Für Ihre Suchanfrage wurde kein Ergebnis gefunden");
            }
        }

        // Click Events
        private void searchConnectionByStartAndDestinationStation (object sender, RoutedEventArgs e) {
            // Save values in Membervariables
            string start = comboBoxStart.Text;
            string destination = comboBoxZiel.Text;
            if (start.Length != 0 && destination.Length != 0 && datePicker.SelectedDate != null && comboBoxTimepicker.SelectedValue != null)  {
                if (datePicker.SelectedDate < new DateTime()) {
                    date = DateTime.Parse(datePicker.SelectedDate.Value.ToString().Substring(0, 8)
                        + " " + comboBoxTimepicker.SelectedValue.ToString());
                    List<Connection> connections = apiSearches.searchForConnection(start, destination, date);
                    // If validation failed
                    if (connections != null) {
                        displayConnectionsOnResultGrid(connections);
                    }
                } else {
                    new TransportException("Verbindungen können nicht angezeigt werden", "Datum darf nicht in der vergangenheit liegen");
                }
              
            } else {
                new TransportException("Verbindungen können nicht angezeigt werden", "Füllen sie alle Felder aus");
            }

        }

        // Click events 
        private void searchStationBoardByStationName(object sender, RoutedEventArgs e) {
            // search for stationboards and display them afterwards
            List<StationBoard> stationBoards = apiSearches.searchForStationBoard(comboBoxBoardSearchValue.Text);
            displayStationBoard(stationBoards);
        }

        private void displayStationBoard (List<StationBoard> stationBoards) {
            // display connections
            foreach (StationBoard station in stationBoards) {
                listBoxAbfahrtsTafelResult.Items.Add(station.To.ToString() + "  :  " + station.Name);
            }
        }

        private void displayConnectionsOnResultGrid (List<Connection> connections) {
            // display connections if matches where found
            if (connections.Count != 0) {
                foreach (Connection connection in connections) {
                    Console.WriteLine(connection.From);
                    listBoxConnectionResult.Items.Add(connection.From.Station.Name + "  -->  " + connection.To.Station.Name.ToString() + "  :  " + connection.Duration);
                }
            }
        }

        // Click event
        private void navigate (object sender, RoutedEventArgs e) {
            // Change tab
            var button = sender as Button;
            navigation.navigate(navigationItems, button.Content.ToString());
        }

       
        // KeyDown event
        private void autocomplateSearchString (object sender, KeyEventArgs e) {
            var comboBox = sender as ComboBox;
            if (comboBox.Text.Length >= 2) {
                buttonSearch.IsEnabled = true;
                buttonGoogleMaps.IsEnabled = true;
            } else {
                buttonSearch.IsEnabled = false;
                buttonGoogleMaps.IsEnabled = false;
            }
            comboBox.IsDropDownOpen = true;
            List<Station> stations = transport.GetStations(comboBox.Text).StationList;
            comboBox.Items.Clear();
            // display all mathes in combobox
            foreach (Station station in stations) {
                if (station.Id != null) {
                    comboBox.Items.Add(station.Name);
                }
            }
        }

        // leave event
        private void setDate (object sender, MouseEventArgs e) {
            var datepicker = sender as DatePicker;
            // manipulate datepicker value
           
        }

        private void setTimeInComboBox () {
            // Date with time 0 hours, 0 minutes and 0 seconds
            DateTime dateTime = new DateTime(2020, 01, 01);
            //Display 25 hours
            for (int i = 0; i < 25; i++) {
                // Add a 0 zero to the hours so it can be parsed to a DateTime objects later
                if (i < 10) {
                    comboBoxTimepicker.Items.Add("0" + dateTime.AddHours(i).Hour + ":00");
                } else {
                    comboBoxTimepicker.Items.Add(dateTime.AddHours(i).Hour + ":00");
                }
            }
        }

        // Show location of a station
        private void showStationLocation (object sender, RoutedEventArgs e) {
            string searchValue = comboBoxStationSearchValue.Text;
            // search station and pull out the x and y coordinates
            List<Station> stations = apiSearches.searchForStation(searchValue).StationList;
            Station station = stations.Find(x => x.Name == searchValue);
            if (station != null) {
                string xCoordinates = station.Coordinate.XCoordinate.ToString().Replace(",", ".");
                string yCoordinates = station.Coordinate.YCoordinate.ToString().Replace(",", ".");
                // start chrome.exe and call google maps api and pass the arguments
                Process.Start("https://www.google.ch/maps/search/?api=1&query=" + xCoordinates + "," + yCoordinates);
            } else {
                new TransportException("Station kann nicht angezeigt werden", "Bitte geben sie einen vollständigen Namen der Station an");
            }
          
        }

        private void selectionChangedStationSearch (object sender, SelectionChangedEventArgs e) {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null) {
                comboBoxStationSearchValue.Text = comboBox.SelectedValue.ToString();
            }
            
        }
    }
}
