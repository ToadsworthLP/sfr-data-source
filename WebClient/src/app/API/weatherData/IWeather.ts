import TemperatureUnit from "@/app/API/weatherData/TemperatureUnit";
import PressureUnit from "@/app/API/weatherData/PressureUnit";
import Provider from "@/app/API/weatherData/Provider";

export default interface IWeather {
    id: string,
    timestamp: Date,
    provider: Provider,
    temperature: number,
    temperatureUnit: TemperatureUnit,
    pressure: number,
    pressureUnit: PressureUnit
}