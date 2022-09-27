import { User } from "../api/dtos/user";
import { WeatherForecast } from "../api/dtos/weather-forecast";

export interface DataFromServer {
    weatherForecasts: WeatherForecast[];
    user: User | null;
}