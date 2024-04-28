import Provider from "@/app/API/weatherData/Provider";
import IWeather from "@/app/API/weatherData/IWeather";

export default class NetworkHandler {
    private readonly host: string;
    private readonly port: number;
    private readonly url: string;

    public constructor(host: string, port: number) {
        this.host = host;
        this.port = port;
        this.url = `http://${this.host}:${this.port}`;
    }

    public async getWeatherForecast(provider: Provider) {
        const totalUrl = `${this.url}/WeatherForecast`

        const response = await fetch(totalUrl);
        const result = await response.json();

        for (const entry of result) {
            entry.timestamp = new Date(entry.timestamp);
        }

        return result as IWeather[];
    }
}