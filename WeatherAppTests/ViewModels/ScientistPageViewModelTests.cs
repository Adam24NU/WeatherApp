using Moq;
using WeatherApp.Core.Models;
using WeatherApp.Core.Repositories;
using WeatherApp.Core.Tools;
using WeatherApp.Core.ViewModels;

namespace WeatherAppTests.ViewModels
{
    public class ScientistPageViewModelTests
    {
        private readonly Mock<DatabaseConnection> _mockDbConnection;
        private Mock<MeasurementRepository> _mockMeasurementRepository;
        private Mock<PhysicalQuantityRepository> _mockPhysicalQuantityRepository;
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly ScientistPageViewModel _viewModel;

        public ScientistPageViewModelTests()
        {
            _mockDbConnection = new Mock<DatabaseConnection>();
            _mockMeasurementRepository = new Mock<MeasurementRepository>(_mockDbConnection.Object);
            _mockPhysicalQuantityRepository = new Mock<PhysicalQuantityRepository>(_mockDbConnection.Object);
            _navigationServiceMock = new Mock<INavigationService>();

            _viewModel = new ScientistPageViewModel(
                _mockPhysicalQuantityRepository.Object,
                _mockMeasurementRepository.Object,
                _navigationServiceMock.Object
            );
        }

        [Fact]
        public async Task OutputMeasurementsAsync_AirSymbol()
        {
            // Arrange
            string symbol = "NO2";
            int quantityId = 1;
            var measurements = new List<Measurement>
            {
                new Measurement { MeasurementId = 1, QuantityId = quantityId, Value = 42.5, Date = "2023-10-01", Time = "12:00" }
            };

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ReturnsAsync(quantityId);

            _mockMeasurementRepository
                .Setup(repo => repo.GetMeasurementsByQIdAsync(quantityId))
                .ReturnsAsync(measurements);

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetUnitByQIdAsync(quantityId))
                .ReturnsAsync("µg/m³");

            // Act
            _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.NotEmpty(_viewModel.AirMeasurements);
            Assert.True(_viewModel.IsAirVisible);
            Assert.False(_viewModel.IsWaterVisible);
            Assert.False(_viewModel.IsWeatherVisible);
            Assert.Equal(1, _viewModel.AirMeasurements.Count);
            Assert.Equal(42.5, _viewModel.AirMeasurements[0].Value);
            Assert.Equal("µg/m³", _viewModel.AirMeasurements[0].Unit);
        }

        [Fact]
        public async Task OutputMeasurementsAsync_WaterSymbol()
        {
            // Arrange
            string symbol = "-NO3";
            int quantityId = 2;
            var measurements = new List<Measurement>
            {
                new Measurement { MeasurementId = 2, QuantityId = quantityId, Value = 15.0, Date = "2023-10-02", Time = "14:00" }
            };

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ReturnsAsync(quantityId);

            _mockMeasurementRepository
                .Setup(repo => repo.GetMeasurementsByQIdAsync(quantityId))
                .ReturnsAsync(measurements);

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetUnitByQIdAsync(quantityId))
                .ReturnsAsync("mg/L");

            // Act
            await _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.NotEmpty(_viewModel.WaterMeasurements);
            Assert.True(_viewModel.IsWaterVisible);
            Assert.False(_viewModel.IsAirVisible);
            Assert.False(_viewModel.IsWeatherVisible);
            Assert.Equal(1, _viewModel.WaterMeasurements.Count);
            Assert.Equal(15.0, _viewModel.WaterMeasurements[0].Value);
            Assert.Equal("mg/L", _viewModel.WaterMeasurements[0].Unit);
        }

        [Fact]
        public async Task OutputMeasurementsAsync_WeatherSymbol()
        {
            // Arrange
            string symbol = "T";
            int quantityId = 3;
            var measurements = new List<Measurement>
            {
                new Measurement { MeasurementId = 3, QuantityId = quantityId, Value = 22.5, Date = "2023-10-03", Time = "16:00" }
            };

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ReturnsAsync(quantityId);

            _mockMeasurementRepository
                .Setup(repo => repo.GetMeasurementsByQIdAsync(quantityId))
                .ReturnsAsync(measurements);

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetUnitByQIdAsync(quantityId))
                .ReturnsAsync("°C");

            // Act
            await _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.NotEmpty(_viewModel.WeatherMeasurements);
            Assert.True(_viewModel.IsWeatherVisible);
            Assert.False(_viewModel.IsAirVisible);
            Assert.False(_viewModel.IsWaterVisible);
            Assert.Equal(1, _viewModel.WeatherMeasurements.Count);
            Assert.Equal(22.5, _viewModel.WeatherMeasurements[0].Value);
            Assert.Equal("°C", _viewModel.WeatherMeasurements[0].Unit);
        }

        [Fact]
        public async Task OutputMeasurementsAsync_InvalidSymbol()
        {
            // Arrange
            string symbol = "INVALID";
            int quantityId = 4;
            var measurements = new List<Measurement>();

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ReturnsAsync(quantityId);

            _mockMeasurementRepository
                .Setup(repo => repo.GetMeasurementsByQIdAsync(quantityId))
                .ReturnsAsync(measurements);

            // Act
            await _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.Empty(_viewModel.AirMeasurements);
            Assert.Empty(_viewModel.WaterMeasurements);
            Assert.Empty(_viewModel.WeatherMeasurements);
            Assert.False(_viewModel.IsAirVisible);
            Assert.False(_viewModel.IsWaterVisible);
            Assert.False(_viewModel.IsWeatherVisible);
        }

        [Fact]
        public async Task OutputMeasurementsAsync_NullQuantityId_ReturnsEmptyList()
        {
            // Arrange
            string symbol = "NO2";

            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ReturnsAsync((int?)null);

            // Act
            await _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.Empty(_viewModel.AirMeasurements);
            Assert.False(_viewModel.IsAirVisible);
            _mockMeasurementRepository.Verify(repo => repo.GetMeasurementsByQIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task NavigateToMapCommand_CallsNavigationService()
        {
            // Arrange
            _navigationServiceMock
                .Setup(nav => nav.NavigateToAsync("ScientistMapPage"))
                .Returns(Task.CompletedTask);

            // Act
            _viewModel.NavigateToMapCommand.Execute(null);

            // Assert
            _navigationServiceMock.Verify(nav => nav.NavigateToAsync("ScientistMapPage"), Times.Once);
        }

        [Fact]
        public async Task OutputMeasurementsAsync_RepositoryThrowsException_DoesNotCrash()
        {
            // Arrange
            string symbol = "NO2";
            _mockPhysicalQuantityRepository
                .Setup(repo => repo.GetQuantityIdBySymbolAsync(symbol))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            await _viewModel.OutputMeasurementsAsync(symbol);

            // Assert
            Assert.Empty(_viewModel.AirMeasurements);
            Assert.False(_viewModel.IsAirVisible);
            Assert.False(_viewModel.IsWaterVisible);
            Assert.False(_viewModel.IsWeatherVisible);
        }
    }
}