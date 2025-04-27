using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using WeatherApp.Models;
using WeatherApp.Repositories;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

public partial class ScientistPageViewModel : ObservableObject
{
    private readonly PhysicalQuantityRepository _physicalQuantityRepository;
    private readonly MeasurementRepository _measurementRepository;

    [ObservableProperty]
    private ObservableCollection<MeasurementDisplay> airMeasurements;

    [ObservableProperty]
    private ObservableCollection<MeasurementDisplay> waterMeasurements;

    [ObservableProperty]
    private ObservableCollection<MeasurementDisplay> weatherMeasurements;

    [ObservableProperty]
    private bool isAirVisible;

    [ObservableProperty]
    private bool isWaterVisible;

    [ObservableProperty]
    private bool isWeatherVisible;

    private readonly INavigationService _navigationService;

    public ICommand LoadMeasurementCommand { get; }
    public ICommand NavigateToMapCommand { get; }

    // Constructor that accepts dependencies (for testing purposes)
    public ScientistPageViewModel(PhysicalQuantityRepository physicalQuantityRepository, MeasurementRepository measurementRepository, INavigationService navigationService)
    {
        _physicalQuantityRepository = physicalQuantityRepository;
        _measurementRepository = measurementRepository;
        _navigationService = navigationService;

        NavigateToMapCommand = new Command(OnNavigateToMap);

        AirMeasurements = new ObservableCollection<MeasurementDisplay>();
        WaterMeasurements = new ObservableCollection<MeasurementDisplay>();
        WeatherMeasurements = new ObservableCollection<MeasurementDisplay>();

        LoadMeasurementCommand = new Command<string>(async (param) => await OutputMeasurementsAsync(param));
    }

    private async void OnNavigateToMap()
    {
        await _navigationService.NavigateToAsync("ScientistMapPage");
    }

    private async Task<List<Measurement>> LoadMeasurementsBySymbolAsync(string symbol)
    {
        try
        {
            Debug.WriteLine($"[DEBUG] Loading measurements for symbol: {symbol}");

            var quantityId = await _physicalQuantityRepository.GetQuantityIdBySymbolAsync(symbol);
            if (!quantityId.HasValue)
            {
                Debug.WriteLine($"[ERROR] Quantity ID not found for symbol: {symbol}");
                return new List<Measurement>();
            }

            var measurements = await _measurementRepository.GetMeasurementsByQIdAsync(quantityId.Value);
            Debug.WriteLine($"[DEBUG] Loaded {measurements.Count} measurements");

            return measurements;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in LoadMeasurementsBySymbolAsync: {ex.Message}\n{ex.StackTrace}");
            throw;
        }
    }

    public async Task OutputMeasurementsAsync(string symbol)
    {
        Debug.WriteLine($"[DEBUG] Starting OutputMeasurementsAsync for: {symbol}");

        List<Measurement> measurements;

        try
        {
            measurements = await LoadMeasurementsBySymbolAsync(symbol);
            if (measurements == null)
            {
                Debug.WriteLine("[ERROR] Measurements list is null!");
                return;
            }

            Debug.WriteLine($"[DEBUG] Retrieved {measurements.Count} measurements in OutputMeasurementsAsync");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in LoadMeasurementsBySymbolAsync: {ex.Message}\n{ex.StackTrace}");
            return;
        }

        var measurementDisplayList = await Task.WhenAll(measurements.Select(async m =>
        {
            if (m.QuantityId == null)
            {
                Debug.WriteLine($"[ERROR] Measurement ID {m.MeasurementId} has null QuantityId.");
                return null;
            }

            string retrievedUnit = await _physicalQuantityRepository.GetUnitByQIdAsync(m.QuantityId);

            if (string.IsNullOrEmpty(retrievedUnit))
            {
                Debug.WriteLine($"[WARN] No symbol found for QuantityId {m.QuantityId}, using '?'");
                retrievedUnit = "?";
            }

            return new MeasurementDisplay
            {
                Date = m.Date,
                Time = m.Time,
                Value = m.Value ?? 0,
                Unit = retrievedUnit
            };
        }));

        var validDisplayList = measurementDisplayList.Where(m => m != null).ToList();

        // Reset all visibility flags and collections
        IsAirVisible = false;
        IsWaterVisible = false;
        IsWeatherVisible = false;
        AirMeasurements = new ObservableCollection<MeasurementDisplay>();
        WaterMeasurements = new ObservableCollection<MeasurementDisplay>();
        WeatherMeasurements = new ObservableCollection<MeasurementDisplay>();

        // Only set measurements and visibility if there are valid measurements
        if (validDisplayList.Any())
        {
            if (symbol is "NO2" or "SO2" or "PM2.5" or "PM10")
            {
                AirMeasurements = new ObservableCollection<MeasurementDisplay>(validDisplayList);
                IsAirVisible = true;
            }
            else if (symbol is "-NO3" or "-NO2" or "-PO4" or "EC")
            {
                WaterMeasurements = new ObservableCollection<MeasurementDisplay>(validDisplayList);
                IsWaterVisible = true;
            }
            else if (symbol is "T" or "H" or "WS" or "WD")
            {
                WeatherMeasurements = new ObservableCollection<MeasurementDisplay>(validDisplayList);
                IsWeatherVisible = true;
            }
            else
            {
                Debug.WriteLine($"[WARN] Symbol '{symbol}' did not match any category.");
            }
        }
        else
        {
            Debug.WriteLine($"[DEBUG] No valid measurements to display for symbol '{symbol}'.");
        }
    }
}
