enum TemperatureUnit {
    Celsius = 0,
    Fahrenheit = 1
}

export function temperatureToString(unit: TemperatureUnit) {
    switch (unit) {
        case TemperatureUnit.Fahrenheit: return "°F";
        case TemperatureUnit.Celsius: return "°C";
    }
}

export default TemperatureUnit;