import "../../css/api.scss";
import React from "react";
import NetworkHandler from "@/app/API/weatherData/NetworkHandler";
import Provider, { providerToString } from "@/app/API/weatherData/Provider";
import IWeather from "@/app/API/weatherData/IWeather";
import TemperatureUnit, { temperatureToString } from "@/app/API/weatherData/TemperatureUnit";
import PressureUnit, { pressureToString } from "@/app/API/weatherData/PressureUnit";

const networkHandler = new NetworkHandler("localhost", 5250);

export default async function API() {
    const forecasts = await networkHandler.getWeatherForecast(Provider.Openmeteo);

    return (
        <div className={"api-container"}>
            <h1 className={"center"}>
                Tem<wbr/>per<wbr/>a<wbr/>ture API
            </h1>
            <table>
                <thead>
                    <tr>
                        <th>Time</th>
                        <th>Provider</th>
                        <th>Temperature</th>
                        <th>Pressure</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map((forecast) => <WeatherEntry key={forecast.id} weather={forecast}/>)}
                </tbody>
            </table>
        </div>
    )
}

function WeatherEntry({ weather }: { weather: IWeather }) {
    return (
        <tr>
            <td>{prettyTimestamp(weather.timestamp)}</td>
            <td>{prettyProvider(weather.provider)}</td>
            <td>{prettyTemperature(weather.temperature, weather.temperatureUnit)}</td>
            <td>{prettyPressure(weather.pressure, weather.pressureUnit)}</td>
        </tr>
    );
}

function prettyTimestamp(date: Date) {
    return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
}

function prettyProvider(provider: Provider) {
    return providerToString(provider);
}

function prettyTemperature(temperature: number, unit: TemperatureUnit) {
    return `${temperature}${temperatureToString(unit)}`;
}

function prettyPressure(pressure: number, unit: PressureUnit) {
    return `${pressure}${pressureToString(unit)}`;
}